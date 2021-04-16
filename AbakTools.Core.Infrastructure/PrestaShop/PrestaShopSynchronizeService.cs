using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Exporters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService : IPrestaShopSynchronizeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        private readonly ISynchronizeStampRepository _synchronizeStampRepository;
        private readonly IPrestaShopClient _prestaShopClient;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITaxRepository _taxRepository;
        private readonly IPrestaShopSynchronizeCustomer _prestaShopSynchronizeCustomer;
        private readonly IPrestaShopSynchronizeOrder _prestaShopSynchronizeOrder;
        private readonly IEnumerable<IPrestaShopExporter> _prestaShopExporters;

        private bool SynchronizationDisabled = false;
        private bool CustomerSynchronizeDisabled = false;


        public PrestaShopSynchronizeService(
            IConfiguration configuration,
            ILogger<PrestaShopSynchronizeService> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampRepository synchronizeStampRepository,
            IPrestaShopClient prestaShopClient,
            ISupplierRepository supplierRepository,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ITaxRepository taxRepository,
            IPrestaShopSynchronizeCustomer prestaShopSynchronizeCustomer,
            IPrestaShopSynchronizeOrder prestaShopSynchronizeOrder,
            IEnumerable<IPrestaShopExporter> prestaShopExporters)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWorkProvider = unitOfWorkProvider;
            _prestaShopClient = prestaShopClient;
            _synchronizeStampRepository = synchronizeStampRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _taxRepository = taxRepository;
            _prestaShopSynchronizeCustomer = prestaShopSynchronizeCustomer;
            _prestaShopSynchronizeOrder = prestaShopSynchronizeOrder;
            _prestaShopExporters = prestaShopExporters;

            bool b;
            SynchronizationDisabled = bool.TryParse(_configuration["PrestaShop:Synchronization:Disabled"], out b) ? b : false;
            CustomerSynchronizeDisabled = bool.TryParse(_configuration["PrestaShop:Synchronization:Customers:Disabled"], out b) ? b : false;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!SynchronizationDisabled)
                    {
                        if (!CustomerSynchronizeDisabled)
                        {
                            await _prestaShopSynchronizeCustomer.DoWork(cancellationToken);
                        }

                        await _prestaShopSynchronizeOrder.DoWork(cancellationToken);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            });

            while (!cancellationToken.IsCancellationRequested)
            {
                if (!SynchronizationDisabled)
                {
                    //_logger.LogDebug("Synchronize suppliers, categories and products");
                    SynchronizeSuppliers();
                    SynchronizeCategories();
                    SynchronizeProduct();

                    if (_prestaShopExporters?.Any() ?? false)
                    {
                        foreach (var exporter in _prestaShopExporters)
                        {
                            await exporter.StartExportAsync(cancellationToken);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }
}
