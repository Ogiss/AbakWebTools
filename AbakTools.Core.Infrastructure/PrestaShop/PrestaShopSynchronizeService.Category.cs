﻿using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Synchronize;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Lib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using AbakTools.Core.Domain;

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
                using (var uow = unitOfWorkProvider.Create())
                {
                    category = categoryRepository.Get(category.Id);
                    category.DisableUpdateModificationDate = true;

                    category psCategory = category.Parent == null
                        ? prestaShopClient.GetRootCategory()
                        : GetPsCategory(category.WebId);

                    if (psCategory == null && category.WebId.HasValue)
                    {
                        category.WebId = null;
                    }

                    if (psCategory == null && (category.Parent == null || category.WebId.HasValue))
                    {
                        throw new PrestaShopSynchronizeException(category.Parent == null
                            ? "Category synchronize error. Didn't find root category"
                            : $"Category synchronize error. Didn't find category with id {category.WebId}");
                    }

                    if (psCategory == null)
                    {
                        if (!category.IsArchived)
                        {
                            psCategory = InsertCategory(category);
                        }
                        else
                        {
                            category.IsDeleted = true;
                        }
                    }
                    else
                    {
                        if (category.IsArchived)
                        {
                            DeleteCategory(category, psCategory);
                        }
                        else
                        {
                            UpdateCategory(category, psCategory);
                        }
                    }

                    if (psCategory != null)
                    {
                        psCategory = SaveOrUpdateCategory(category, psCategory);
                    }

                    if (!category.WebId.HasValue)
                    {
                        category.WebId = (int?)psCategory?.id;
                    }

                    category.Synchronize = Framework.SynchronizeType.Synchronized;
                    categoryRepository.SaveOrUpdate(category);
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Synchronize category Id:{category.Id} error.{Environment.NewLine}{ex}");
            }
            finally
            {
                category.DisableUpdateModificationDate = false;
            }
        }

        private category GetPsCategory(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    return prestaShopClient.CategoryFactory.Get(id.Value);
                }
                catch (Bukimedia.PrestaSharp.PrestaSharpException ex)
                {
                    if (ex.ResponseHttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }

                    throw;
                }
            }

            return null;
        }

        private category InsertCategory(CategoryEntity category)
        {
            var psCategory = new category();
            psCategory.id_parent = (long)category.Parent.WebId.Value;
            UpdateCategory(category, psCategory);

            return psCategory;
        }

        private void UpdateCategory(CategoryEntity category, category psCategory)
        {
            if (psCategory.is_root_category == 0)
            {
                prestaShopClient.SetLangValue(psCategory, x => x.name, Functions.GetPrestaShopName(category.Name));
                prestaShopClient.SetLangValue(psCategory, x => x.link_rewrite, Functions.GetLinkRewrite(category.Name));
                psCategory.active = (category.Active ?? false && !category.IsDeleted) ? 1 : 0;
            }
        }

        private void DeleteCategory(CategoryEntity category, category psCategory)
        {
            if (psCategory.is_root_category == 0)
            {
                psCategory.active = 0;
                category.IsDeleted = true;
            }
        }

        private category SaveOrUpdateCategory(CategoryEntity category, category psCategory)
        {
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

            return psCategory;
        }
    }
}
