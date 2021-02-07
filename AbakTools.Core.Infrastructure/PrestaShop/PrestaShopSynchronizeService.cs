using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService : IPrestaShopSynchronizeService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ISynchronizeStampRepository synchronizeStampRepository;
        private readonly IPrestaShopClient prestaShopClient;
        private readonly ISupplierRepository supplierRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;
        private readonly ITaxRepository taxRepository;
        private readonly IPrestaShopSynchronizeCustomer prestaShopSynchronizeCustomer;
        private readonly IPrestaShopSynchronizeOrder prestaShopSynchronizeOrder;

        private bool SynchronizationDisabled = false;
        private bool CustomerSynchronizeDisabled = false;


        public PrestaShopSynchronizeService(
            IConfiguration _configuration,
            ILogger<PrestaShopSynchronizeService> _logger,
            IUnitOfWorkProvider _unitOfWorkProvider,
            ISynchronizeStampRepository _synchronizeStampRepository,
            IPrestaShopClient _prestaShopClient,
            ISupplierRepository _supplierRepository,
            ICategoryRepository _categoryRepository,
            IProductRepository _productRepository,
            ITaxRepository _taxRepository,
            IPrestaShopSynchronizeCustomer _prestaShopSynchronizeCustomer,
            IPrestaShopSynchronizeOrder _prestaShopSynchronizeOrder)
        {
            configuration = _configuration;
            logger = _logger;
            unitOfWorkProvider = _unitOfWorkProvider;
            prestaShopClient = _prestaShopClient;
            synchronizeStampRepository = _synchronizeStampRepository;
            supplierRepository = _supplierRepository;
            categoryRepository = _categoryRepository;
            productRepository = _productRepository;
            taxRepository = _taxRepository;
            prestaShopSynchronizeCustomer = _prestaShopSynchronizeCustomer;
            prestaShopSynchronizeOrder = _prestaShopSynchronizeOrder;

            bool b;
            SynchronizationDisabled = bool.TryParse(configuration["PrestaShop:Synchronization:Disabled"], out b) ? b : false;
            CustomerSynchronizeDisabled = bool.TryParse(configuration["PrestaShop:Synchronization:Customers:Disabled"], out b) ? b : false;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (!SynchronizationDisabled)
                    {
                        if (!CustomerSynchronizeDisabled)
                        {
                            await prestaShopSynchronizeCustomer.DoWork(stoppingToken);
                        }

                        await prestaShopSynchronizeOrder.DoWork(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!SynchronizationDisabled)
                {
                    logger.LogDebug("Synchronize suppliers, categories and products");
                    SynchronizeSuppliers();
                    SynchronizeCategories();
                    SynchronizeProduct();
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
