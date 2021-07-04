using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    internal class PsMessageRepository : PsRepositoryBase<message>, IPsMessageRepository
    {
        protected override GenericFactory<message> Factory => PrestaShopClient.MessageFactory;

        public PsMessageRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }
    }
}
