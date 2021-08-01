using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Product.Repositories;
using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Exporters;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using AbakTools.Core.Infrastructure.PrestaShop.Services;
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
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        private readonly ISynchronizeStampRepository _synchronizeStampRepository;
        private readonly IPrestaShopClient _prestaShopClient;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ITaxRepository _taxRepository;
        private readonly IPsProductDiscountGroupRepository _psProductDiscountGroupRepository;
        private readonly IDiscountGroupSynchronizeService _discountGroupSynchronizeService;
        private readonly IPrestaShopSynchronizeCustomer _prestaShopSynchronizeCustomer;
        private readonly IPrestaShopSynchronizeOrder _prestaShopSynchronizeOrder;
        private readonly IEnumerable<IPrestaShopExporter> _prestaShopExporters;

        private bool SynchronizationDisabled = false;
        private bool CustomerSynchronizeDisabled = false;


        public PrestaShopSynchronizeService(
            IConfiguration configuration,
            ILogger<PrestaShopSynchronizeService> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            IServiceProvider serviceProvider,
            ISynchronizeStampRepository synchronizeStampRepository,
            IPrestaShopClient prestaShopClient,
            ISupplierRepository supplierRepository,
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            ITaxRepository taxRepository,
            IPsProductDiscountGroupRepository psProductDiscountGroupRepository,
            IDiscountGroupSynchronizeService discountGroupSynchronizeService,
            IPrestaShopSynchronizeCustomer prestaShopSynchronizeCustomer,
            IPrestaShopSynchronizeOrder prestaShopSynchronizeOrder,
            
            IEnumerable<IPrestaShopExporter> prestaShopExporters)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWorkProvider = unitOfWorkProvider;
            _serviceProvider = serviceProvider;
            _prestaShopClient = prestaShopClient;
            _synchronizeStampRepository = synchronizeStampRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _taxRepository = taxRepository;
            _psProductDiscountGroupRepository = psProductDiscountGroupRepository;
            _discountGroupSynchronizeService = discountGroupSynchronizeService;
            _prestaShopSynchronizeCustomer = prestaShopSynchronizeCustomer;
            _prestaShopSynchronizeOrder = prestaShopSynchronizeOrder;
            _prestaShopExporters = prestaShopExporters;

            bool b;
            SynchronizationDisabled = bool.TryParse(_configuration["PrestaShop:Synchronization:Disabled"], out b) ? b : false;
            CustomerSynchronizeDisabled = bool.TryParse(_configuration["PrestaShop:Synchronization:Customers:Disabled"], out b) ? b : false;
        }

        public Task DoWork(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                _prestaShopSynchronizeCustomer.DoWork(cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                SynchronizeSuppliers();

            if (!cancellationToken.IsCancellationRequested)
                SynchronizeCategories();

            if (_prestaShopExporters?.Any() ?? false && !cancellationToken.IsCancellationRequested)
            {
                foreach (var exporter in _prestaShopExporters)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    exporter.StartExport(cancellationToken);
                }
            }

            return Task.CompletedTask;
        }
    }
}
