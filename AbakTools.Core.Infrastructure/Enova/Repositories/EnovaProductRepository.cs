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
    class EnovaProductRepository : EnovaGenericEntityRepository<EnovaProduct>, IEnovaProductRepository
    {
        protected override string Resource => ResourcesNames.Products;

        public EnovaProductRepository(IEnovaAPiClient enovaAPiClient) : base(enovaAPiClient) { }

        public ProductPriceInfo GetPriceForCustomer(Guid productGuid, Guid customerGuid)
        {
            return Api.GetValueAsync<ProductPriceInfo>(
                Resource,
                productGuid,
                EnovaApiProduct.ProductAssociationsNames.CustomerPrices,
                customerGuid).Result;
        }

        public async Task<IEnumerable<EnovaApiProduct.Price>> GetModifiedPricesAsync(Guid definitionGuid, long stampFrom, long stampTo)
        {
            return await Api.GetValueAsync<IEnumerable<EnovaApiProduct.Price>>(
                ResourcesNames.ProductsModifiedPrices, $"{definitionGuid}/{stampFrom}/{stampTo}");
        }
    }
}
