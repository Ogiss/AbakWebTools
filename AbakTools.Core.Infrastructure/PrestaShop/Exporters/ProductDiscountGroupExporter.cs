using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Services;
using AbakTools.Core.Framework.Helpers.Extensions;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PsProductDiscountGroup = Bukimedia.PrestaSharp.Entities.product_discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    class ProductDiscountGroupExporter : PrestaShopExporterBase<int>
    {
        private readonly IProductDiscountGroupRepository _productDiscountGroupRepository;
        private readonly IPSProductDiscountGroupRepository _psProductDiscountGroupRepository;
        private readonly IProductRepository _productRepository;

        public ProductDiscountGroupExporter(
            ILogger<ProductDiscountGroupExporter> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampService synchronizeStampService,
            IProductDiscountGroupRepository productDiscountGroupRepository,
            IPSProductDiscountGroupRepository psProductDiscountGroupRepository,
            IProductRepository productRepository)
            : base(logger, unitOfWorkProvider, synchronizeStampService)
        {
            _productDiscountGroupRepository = productDiscountGroupRepository;
            _psProductDiscountGroupRepository = psProductDiscountGroupRepository;
            _productRepository = productRepository;
        }

        protected async override Task<IEnumerable<int>> GetExportingEntriesAsync(CancellationToken cancelerationToken)
        {
            using (UnitOfWorkProvider.CreateReadOnly())
            {
                return await _productDiscountGroupRepository.GetEnovaProductIdsWithModifiedDiscountGroupsAsync(StampFrom, StampTo);
            }
        }

        protected override void ProcessEntry(int enovaProductId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                _productDiscountGroupRepository
                    .GetAllGroupsForProductWithReferences(enovaProductId)
                    .Foreach(ProcessProductDiscountGroup);

                uow.Commit();
            }
        }

        private void ProcessProductDiscountGroup(Domain.Product.ProductDiscountGroupEntity group)
        {
            if (group.DiscountGroup.WebId > 0 && group.Product.EnovaGuid.HasValue)
            {
                _productRepository
                    .GetAllWebIdsByEnovaGuid(group.Product.EnovaGuid.Value)
                    .Select(x => (x, group))
                    .Foreach(ProcessProductWebId);
            }
        }

        private void ProcessProductWebId((int productWebId, ProductDiscountGroupEntity group) entry)
        {
            var psProductGroups = _psProductDiscountGroupRepository.Get(entry.productWebId, entry.group.DiscountGroup.WebId.Value);

            if (entry.group.IsArchived)
            {
                DeleteProductDiscountGroup(entry.group, psProductGroups);
            }
            else
            {
                if (psProductGroups == null)
                {
                    psProductGroups = InsertProductDiscountGroup(entry.productWebId, entry.group);
                }
                else
                {
                    UpdateProductDiscountGroup(entry.group, psProductGroups);
                }
            }

            entry.group.MakeSynchronized();
            _productDiscountGroupRepository.SaveOrUpdate(entry.group);
        }

        private PsProductDiscountGroup InsertProductDiscountGroup(int productWebId, ProductDiscountGroupEntity group)
        {
            Logger.LogDebug($"Insert discount group {group.DiscountGroup.Name} for product web id {productWebId}.");

            var psDiscountGroup = new PsProductDiscountGroup
            {
                id_product = productWebId,
                id_discount_group = group.DiscountGroup.WebId.Value
            };

            return _psProductDiscountGroupRepository.SaveOrUpdate(psDiscountGroup);

        }

        private void UpdateProductDiscountGroup(ProductDiscountGroupEntity group, PsProductDiscountGroup psProductGroups)
        {
            // TODO: product and group can't be changed
        }

        private void DeleteProductDiscountGroup(ProductDiscountGroupEntity group, PsProductDiscountGroup psProductGroups)
        {
            if (psProductGroups != null)
            {
                _psProductDiscountGroupRepository.Delete(psProductGroups);
            }
        }
    }
}
