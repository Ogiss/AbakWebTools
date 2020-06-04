using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Synchronize;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Lib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService
    {
        private void SynchronizeCategories()
        {
            SynchronizeStampEntity synchronizeStamp = null;
            DateTime stampTo = DateTime.Now;

            using (var uow = unitOfWorkProvider.CreateReadOnly())
            {
                synchronizeStamp = synchronizeStampRepository.Get(SynchronizeCodes.Category, SynchronizeDirectionType.Export);
            }

            DateTime stampFrom = synchronizeStamp?.DateTimeStamp ?? DateTime.MinValue;

            IReadOnlyCollection<CategoryEntity> categories = null;

            using (var uow = unitOfWorkProvider.CreateReadOnly())
            {
                categories = categoryRepository.GetAllModified(stampFrom, null);
            }

            if (categories.Any())
            {
                logger.LogDebug("Starting synchronize categories");

                stampTo = categories.Max(x => x.ModificationDate);

                foreach (var category in categories)
                {
                    ProcessCategory(category);
                }

                if (synchronizeStamp == null)
                {
                    synchronizeStamp = SynchronizeStampFactory.Create(SynchronizeCodes.Category, SynchronizeDirectionType.Export);
                }

                synchronizeStamp.DateTimeStamp = stampTo;

                using (var uow = unitOfWorkProvider.Create())
                {
                    synchronizeStampRepository.SaveOrUpdate(synchronizeStamp);
                    uow.Commit();
                }

                logger.LogDebug($"Synchronize categories finished at {(DateTime.Now - stampTo).TotalSeconds} sec.");
            }
        }

        private void ProcessCategory(CategoryEntity category)
        {
            try
            {
                category psCategory = category.Parent == null
                    ? prestaShopClient.GetRootCategory()
                    : (category.WebId.HasValue ? prestaShopClient.CategoryFactory.Get(category.WebId.Value) : null);

                if (psCategory == null && (category.Parent == null || category.WebId.HasValue))
                {
                    throw new PrestaShopSynchronizeException(category.Parent == null
                        ? "Category synchronize error. Didn't find root category"
                        : $"Category synchronize error. Didn't find category with id {category.WebId}");
                }


                if (psCategory == null)
                {
                    psCategory = new category();
                    psCategory.id_parent = (long)category.Parent.WebId.Value;
                }

                if (psCategory.is_root_category == 0)
                {
                    prestaShopClient.SetLangValue(psCategory, x => x.name, Functions.GetPrestaShopName(category.Name));
                    prestaShopClient.SetLangValue(psCategory, x => x.link_rewrite, Functions.GetLinkRewrite(category.Name));


                    psCategory.active = (category.Active ?? false && !category.IsDeleted) ? 1 : 0;

                    if (psCategory.id.HasValue && psCategory.id.Value > 0)
                    {
                        logger.LogInformation($"Update category id: {category.Id}, name: {category.Name}");
                        prestaShopClient.CategoryFactory.Update(psCategory);
                    }
                    else
                    {
                        logger.LogInformation($"Add new category id: {category.Id}, name: {category.Name}");
                        psCategory = prestaShopClient.CategoryFactory.Add(psCategory);
                    }
                }

                if (!category.WebId.HasValue)
                {
                    category.WebId = (int)psCategory.id;

                    using (var uow = unitOfWorkProvider.Create())
                    {
                        categoryRepository.SaveOrUpdate(category);
                        uow.Commit();
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError($"Synchronize category Id:{category.Id} error.{Environment.NewLine}{ex}");
            }
        }
    }
}
