using Bukimedia.PrestaSharp.Factories;
using System.Linq;
using PSProductDiscountGroup = Bukimedia.PrestaSharp.Entities.product_discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    class PSProductDiscountGroupRepository : PSRepositoryBase<PSProductDiscountGroup>, IPSProductDiscountGroupRepository
    {
        protected override GenericFactory<PSProductDiscountGroup> Factory => PrestaShopClient.ProductDiscountGroupFactory;

        public PSProductDiscountGroupRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }

        public PSProductDiscountGroup Get(int productId, int discountGroupId)
        {
            return GetByFilter(new { id_product = productId.ToString(), id_discount_group = discountGroupId.ToString() }).SingleOrDefault();
        }
    }
}
