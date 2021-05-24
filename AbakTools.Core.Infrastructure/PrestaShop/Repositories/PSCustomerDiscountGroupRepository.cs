using Bukimedia.PrestaSharp.Factories;
using System.Linq;
using PSCustomerDiscountGroup = Bukimedia.PrestaSharp.Entities.customer_discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    class PSCustomerDiscountGroupRepository : PSRepositoryBase<PSCustomerDiscountGroup>, IPSCustomerDiscountGroupRepository
    {
        protected override GenericFactory<PSCustomerDiscountGroup> Factory => PrestaShopClient.CustomerDiscountGroupFactory;

        public PSCustomerDiscountGroupRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }

        public PSCustomerDiscountGroup Get(int customerId, int discountGroupId)
        {
            return GetByFilter(new { id_customer = customerId.ToString(), id_discount_group = discountGroupId.ToString() }).SingleOrDefault();
        }
    }
}
