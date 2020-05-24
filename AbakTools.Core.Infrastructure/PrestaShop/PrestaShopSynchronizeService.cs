using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService : IPrestaShopSynchronizeService
    {
        private readonly ILogger logger;
        private IUnitOfWorkProvider unitOfWorkProvider;
        private ISynchronizeStampRepository synchronizeStampRepository;
        private IPrestaShopClient prestaShopClient;
        private readonly ISupplierRepository supplierRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;
        private readonly ITaxRepository taxRepository;


        public PrestaShopSynchronizeService(
            ILogger<PrestaShopSynchronizeService> _logger,
            IUnitOfWorkProvider _unitOfWorkProvider,
            ISynchronizeStampRepository _synchronizeStampRepository,
            IPrestaShopClient _prestaShopClient,
            ISupplierRepository _supplierRepository,
            ICategoryRepository _categoryRepository,
            IProductRepository _productRepository,
            ITaxRepository _taxRepository)
        {
            logger = _logger;
            unitOfWorkProvider = _unitOfWorkProvider;
            prestaShopClient = _prestaShopClient;
            synchronizeStampRepository = _synchronizeStampRepository;
            supplierRepository = _supplierRepository;
            categoryRepository = _categoryRepository;
            productRepository = _productRepository;
            taxRepository = _taxRepository;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Synchronize suppliers");
                SynchronizeSuppliers();
                SynchronizeCategories();

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
