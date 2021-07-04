using PSCustomerDiscountGroup = Bukimedia.PrestaSharp.Entities.customer_discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    public interface IPsCustomerDiscountGroupRepository : IPsRepositoryBase<PSCustomerDiscountGroup>
    {
        PSCustomerDiscountGroup Get(int customerId, int discountGroupId);
    }
}
