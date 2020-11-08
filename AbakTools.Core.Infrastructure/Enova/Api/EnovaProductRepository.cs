using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.Enova.Product;
using Enova.Api;
using EnovaApiProduct = EnovaApi.Models.Product;
using System;

namespace AbakTools.Core.Infrastructure.Enova.Api
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
    }
}
