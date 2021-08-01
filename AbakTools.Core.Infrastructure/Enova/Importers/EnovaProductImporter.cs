using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Enova.Product;
using AbakTools.Core.Domain.Product.Repositories;
using EnovaApi.Models.Product;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal class EnovaProductImporter : EnovaImporterWithDateTimeStampBase<Guid>
    {
        private readonly ILogger _logger;
        private readonly IEnovaProductRepository _enovaProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountGroupRepository _discountGroupRepository;

        protected override ILogger Logger => _logger;

        public EnovaProductImporter(ILogger<EnovaProductImporter> logger, IEnovaProductRepository enovaProductRepository, IProductRepository productRepository, IDiscountGroupRepository discountGroupRepository)
            => (_logger, _enovaProductRepository, _productRepository, _discountGroupRepository) = (logger, enovaProductRepository, productRepository, discountGroupRepository);

        protected override IEnumerable<Guid> GetEntries(DateTime stampFrom, DateTime stampTo)
        {
            using var uow = UnitOfWorkProvider.CreateReadOnly();
            return _enovaProductRepository.GetModifiedProductsGuidsAsync(StampFrom, stampTo).Result;
        }

        protected override void ProcessEntry(Guid guid)
        {
            using var uow = UnitOfWorkProvider.Create();
            var enovaProduct = GetProductFromEnovaApi(guid);
            var products = _productRepository.GetEnovaProductsWithoutInDeletingProcessAsync(guid).Result;
            var idx = 0;

            if (products.Any())
            {
                foreach (var product in products)
                {
                    ProcessProduct(product, enovaProduct, isDuplicated: idx++ > 0);
                }
            }
            else
            {
                // TODO: INSERT PRODUCT
            }

            uow.Commit();
        }

        private Product GetProductFromEnovaApi(Guid guid)
        {
            try
            {
                return _enovaProductRepository.Get(guid).Result;
            }
            catch (AggregateException ex)
            {
                ex.Handle(x =>
                {
                    if (x is HttpRequestException rqEx && rqEx.StatusCode == HttpStatusCode.NotFound)
                    {
                        return true;
                    }

                    return false;
                });
            }

            return null;
        }

        private void ProcessProduct(Domain.Product.ProductEntity product, Product enovaProduct, bool isDuplicated)
        {
            _logger.LogDebug($"Updating enova product Id: {product.Id}, {product.Code} - {product.Name}");

            if (enovaProduct != null)
            {
                product.SetCode(enovaProduct.Code);
                product.SetName(enovaProduct.Name);
            }

            if (isDuplicated || enovaProduct == null)
            {
                product.StartDeletingProcess();
            }
            else
            {
                UpdateDiscountGroups(product, enovaProduct);
            }

            _productRepository.SaveOrUpdate(product);
        }

        private void UpdateDiscountGroups(Domain.Product.ProductEntity product, Product enovaProduct)
        {
            try
            {
                var groupGuids = enovaProduct.DiscountGroups.Select(x => x.Guid).ToArray();

                foreach (var groupGuid in groupGuids)
                {
                    var productGroup = product.ProductDiscountGroups.SingleOrDefault(x => x.DiscountGroup.Guid == groupGuid);

                    if (productGroup == null)
                    {
                        var discountGroup = _discountGroupRepository.Get(groupGuid);
                        product.AddDiscountGroup(discountGroup);
                    }

                    if (productGroup?.IsArchived ?? false)
                    {
                        productGroup.Restore();
                    }
                }

                foreach (var productGroup in product.ProductDiscountGroups.Where(x => !x.IsArchived && !groupGuids.Contains(x.DiscountGroup.Guid)).ToList())
                {
                    productGroup.StartDeletingProcess();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurrence when update product Id: {product.Id}, {product.Code} - {product.Name}.{Environment.NewLine}{ex}");
            }
        }
    }
}
