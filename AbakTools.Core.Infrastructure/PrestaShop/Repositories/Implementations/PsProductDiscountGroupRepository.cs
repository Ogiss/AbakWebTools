using Bukimedia.PrestaSharp.Factories;
using System.Linq;
using PSProductDiscountGroup = Bukimedia.PrestaSharp.Entities.product_discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    class PsProductDiscountGroupRepository : PsRepositoryBase<PSProductDiscountGroup>, IPsProductDiscountGroupRepository
    {
        protected override GenericFactory<PSProductDiscountGroup> Factory => PrestaShopClient.ProductDiscountGroupFactory;

        public PsProductDiscountGroupRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }

        public PSProductDiscountGroup Get(int productId, int discountGroupId)
        {
            return GetByFilter(new { id_product = productId.ToString(), id_discount_group = discountGroupId.ToString() }).SingleOrDefault();
        }
    }
}
