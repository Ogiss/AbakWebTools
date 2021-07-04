using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Services;
using AbakTools.Core.Framework.Helpers.Extensions;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    class ProductDiscountGroupExporter : PrestaShopExporterBase<int>
    {
        private readonly IProductDiscountGroupSynchronizeService _productDiscountGroupSynchronizeService;
        private readonly IProductDiscountGroupRepository _productDiscountGroupRepository;
        private readonly IProductRepository _productRepository;

        public ProductDiscountGroupExporter(
            ILogger<ProductDiscountGroupExporter> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampService synchronizeStampService,
            IProductDiscountGroupSynchronizeService productDiscountGroupSynchronizeService,
            IProductDiscountGroupRepository productDiscountGroupRepository,
            IProductRepository productRepository)
            : base(logger, unitOfWorkProvider, synchronizeStampService)
        {
            _productDiscountGroupSynchronizeService = productDiscountGroupSynchronizeService;
            _productDiscountGroupRepository = productDiscountGroupRepository;
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

                group.MakeSynchronized();
                _productDiscountGroupRepository.SaveOrUpdate(group);
            }
        }

        private void ProcessProductWebId((int productWebId, ProductDiscountGroupEntity group) entry)
        {
            _productDiscountGroupSynchronizeService.Synchronize(entry.productWebId, entry.group);
        }
    }
}
