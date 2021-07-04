using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    class PsProductRepository : PsRepositoryBase<Bukimedia.PrestaSharp.Entities.product>, IPsProductRepository
    {
        protected override GenericFactory<product> Factory => PrestaShopClient.ProductFactory;

        public PsProductRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }
    }
}
