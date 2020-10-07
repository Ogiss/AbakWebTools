using AbakTools.Core.Domain.Common;
using System;

namespace AbakTools.Core.Domain.Enova.Product
{
    public interface IEnovaProductRepository : IGenericEntityRepository<EnovaProduct>
    {
        ProductPriceInfo GetPriceForCustomer(Guid productGuid, Guid customerGuid);
    }
}
