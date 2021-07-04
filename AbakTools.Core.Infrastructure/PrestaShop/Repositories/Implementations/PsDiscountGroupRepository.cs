using Bukimedia.PrestaSharp.Factories;
using PSDiscountGroup = Bukimedia.PrestaSharp.Entities.discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    class PsDiscountGroupRepository : PsRepositoryBase<PSDiscountGroup>, IPsDiscountGroupRepository
    {
        protected override GenericFactory<PSDiscountGroup> Factory => PrestaShopClient.DiscountGroupFactory;

        public PsDiscountGroupRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }
    }
}
