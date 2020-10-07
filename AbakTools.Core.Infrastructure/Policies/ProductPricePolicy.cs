using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Enova.Product;
using AbakTools.Core.Domain.Policies;
using AbakTools.Core.Domain.Product;

namespace AbakTools.Core.Infrastructure.Policies
{
    class ProductPricePolicy : IProductPricePolicy
    {
        private IEnovaProductRepository _enovaProductRepository;

        public ProductPricePolicy(IEnovaProductRepository enovaProductRepository)
        {
            _enovaProductRepository = enovaProductRepository;
        }

        public ProductPriceInfo GetProductPriceInfoForCustomer(ProductEntity product, CustomerEntity customer)
        {
            return _enovaProductRepository.GetPriceForCustomer(product.EnovaGuid.Value, customer.Guid);
        }
    }
}
