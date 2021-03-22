using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.Enova.Product;
using Enova.Api;
using EnovaApiProduct = EnovaApi.Models.Product;
using System;
using AbakTools.Core.Infrastructure.Enova.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    class EnovaProductRepository : IEnovaProductRepository
    {
        private readonly IEnovaAPiClient _api;
        protected string Resource => ResourcesNames.Products;

        public EnovaProductRepository(IEnovaAPiClient api) => _api = api;

        public ProductPriceInfo GetPriceForCustomer(Guid productGuid, Guid customerGuid)
        {
            return _api.GetValueAsync<ProductPriceInfo>(
                Resource,
                productGuid,
                EnovaApiProduct.ProductAssociationsNames.CustomerPrices,
                customerGuid).Result;
        }

        public async Task<IEnumerable<EnovaApiProduct.Price>> GetModifiedPricesAsync(Guid definitionGuid, long stampFrom, long stampTo)
        {
            return await _api.GetValueAsync<IEnumerable<EnovaApiProduct.Price>>(
                ResourcesNames.ProductsModifiedPrices, $"{definitionGuid}/{stampFrom}/{stampTo}");
        }

        public async Task<IEnumerable<Guid>> GetModifiedProductsGuidsAsync(DateTime stampFrom, DateTime stampTo)
        {
            return await _api.GetValueAsync<IEnumerable<Guid>>(
                ResourcesNames.ProductsModifiedGuids, $"{stampFrom}/{stampTo}");
        }

        public async Task<EnovaApiProduct.Product> Get(Guid guid)
        {
            return await _api.GetValueAsync<EnovaApiProduct.Product>(ResourcesNames.Products, guid.ToString());
        }
    }
}
