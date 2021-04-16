using Bukimedia.PrestaSharp.Factories;
using PSDiscountGroup = Bukimedia.PrestaSharp.Entities.discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    class PSDiscountGroupRepository : PSRepositoryBase<PSDiscountGroup>, IPSDiscountGroupRepository
    {
        protected override GenericFactory<PSDiscountGroup> Factory => PrestaShopClient.DiscountGroupFactory;

        public PSDiscountGroupRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }
    }
}
