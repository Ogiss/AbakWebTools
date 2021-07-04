using AbakTools.Core.Domain.Common.Projections;
using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Product.Specifications;
using AbakTools.Core.Domain.Services;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    class ProductExporter : PrestaShopExporterBase<int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSynchronizeService _productSynchronizeService;

        public ProductExporter(
            ILogger<ProductExporter> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampService synchronizeStampService,
            IProductRepository productRepository,
            IProductSynchronizeService productSynchronizeService) : base(logger, unitOfWorkProvider, synchronizeStampService)
        {
            _productRepository = productRepository;
            _productSynchronizeService = productSynchronizeService;
        }

        protected override async Task<IEnumerable<int>> GetExportingEntriesAsync(CancellationToken cancelerationToken)
        {
            using (UnitOfWorkProvider.CreateReadOnly())
            {
                return await _productRepository.GetListAsync(ProductToExportSpecification.Instance, EntityIdProjection<ProductEntity>.Create());
            }
        }

        protected override void ProcessEntry(int id)
        {
            using var uow = UnitOfWorkProvider.Create();
            var product = _productRepository.Get(id);
            _productSynchronizeService.Synchronize(product);
            uow.Commit();
        }
    }
}
