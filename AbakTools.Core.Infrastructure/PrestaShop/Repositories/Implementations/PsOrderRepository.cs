using Bukimedia.PrestaSharp.Factories;
using PsOrder = Bukimedia.PrestaSharp.Entities.order;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    internal class PSOrderRepository : PsRepositoryBase<PsOrder>, IPSOrderRepository
    {
        protected override GenericFactory<PsOrder> Factory => PrestaShopClient.OrderFactory;

        public PSOrderRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }
    }
}
