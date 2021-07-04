using PsProductDiscountGroup = Bukimedia.PrestaSharp.Entities.product_discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    public interface IPsProductDiscountGroupRepository : IPsRepositoryBase<PsProductDiscountGroup>
    {
        PsProductDiscountGroup Get(int productId, int discountGroupId);
    }
}
