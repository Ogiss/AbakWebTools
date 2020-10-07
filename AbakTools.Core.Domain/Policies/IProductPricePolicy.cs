using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Product;

namespace AbakTools.Core.Domain.Policies
{
    public interface IProductPricePolicy
    {
        ProductPriceInfo GetProductPriceInfoForCustomer(ProductEntity product, CustomerEntity customer);
    }
}
