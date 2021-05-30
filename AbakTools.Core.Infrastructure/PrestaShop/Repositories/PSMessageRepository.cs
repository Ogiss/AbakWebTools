using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    internal class PSMessageRepository : PSRepositoryBase<message>, IPSMessageRepository
    {
        protected override GenericFactory<message> Factory => PrestaShopClient.MessageFactory;

        public PSMessageRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }
    }
}
