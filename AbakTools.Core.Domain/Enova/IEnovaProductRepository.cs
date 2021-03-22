using EnovaApi.Models.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductPriceInfo = AbakTools.Core.Domain.Common.ProductPriceInfo;

namespace AbakTools.Core.Domain.Enova.Product
{
    public interface IEnovaProductRepository
    {
        ProductPriceInfo GetPriceForCustomer(Guid productGuid, Guid customerGuid);
        Task<IEnumerable<Price>> GetModifiedPricesAsync(Guid definitionGuid, long stampFrom, long stampTo);
        Task<IEnumerable<Guid>> GetModifiedProductsGuidsAsync(DateTime stampFrom, DateTime stampTo);
        Task<EnovaApi.Models.Product.Product> Get(Guid guid);
    }
}
