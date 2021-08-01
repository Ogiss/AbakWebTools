using AbakTools.Core.Domain.Enova.Product;
using AbakTools.Core.Domain.Product.Repositories;
using EnovaApi.Models.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal class EnovaPricesImporter : EnovaImporterWithLongStampBase<Price>
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IEnovaProductRepository _enovaProductRepository;
        private readonly IProductRepository _productRepository;

        protected override ILogger Logger => _logger;

        public EnovaPricesImporter(
            ILogger<EnovaPricesImporter> logger,
            IConfiguration configuration,
            IEnovaProductRepository enovaProductRepository,
            IProductRepository productRepository)

            => (_logger, _configuration, _enovaProductRepository, _productRepository) 
            = (logger, configuration, enovaProductRepository, productRepository);

        protected override IEnumerable<Price> GetEntries(long stampFrom, long stampTo)
        {
            using var uow = UnitOfWorkProvider.CreateReadOnly();
            var priceDefGuid = Guid.Parse(_configuration.GetSection("EnovaSynchronization:DefaultPriceGuid").Value);
            return _enovaProductRepository.GetModifiedPricesAsync(priceDefGuid, stampFrom, stampTo).Result;
        }

        protected override void ProcessEntry(Price entry)
        {
            using var uow = UnitOfWorkProvider.Create();
            var products = _productRepository.GetAllByEnovaGuid(entry.ProductGuid);

            foreach(var product in products)
            {
                _logger.LogDebug($"Update price for {product.Code} - {product.Name}");
                product.SetPrice(entry.PriceWithoutTax);
                _productRepository.SaveOrUpdate(product);
            }

            uow.Commit();
        }

        protected override void HandleProcessEntryException(Price entry, Exception exception)
        {
            _logger.LogError($"Error occurred during import price entry ({entry}).{Environment.NewLine}{exception}");
        }

        protected override void HandleImportExeception(Exception exception)
        {
            _logger.LogError(exception.ToString());
        }
    }
}
