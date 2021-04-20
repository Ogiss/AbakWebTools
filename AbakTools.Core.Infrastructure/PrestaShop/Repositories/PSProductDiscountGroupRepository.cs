using Bukimedia.PrestaSharp.Factories;
using System.Linq;
using PsProductDiscountGroup = Bukimedia.PrestaSharp.Entities.product_discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    class PSProductDiscountGroupRepository : PSRepositoryBase<PsProductDiscountGroup>, IPSProductDiscountGroupRepository
    {
        protected override GenericFactory<PsProductDiscountGroup> Factory => PrestaShopClient.ProductDiscountGroupFactory;

        public PSProductDiscountGroupRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }

        public PsProductDiscountGroup Get(int productId, int discountGroupId)
        {
            return GetByFilter(new { id_product = productId.ToString(), id_discount_group = discountGroupId.ToString() }).SingleOrDefault();
        }
    }
}
