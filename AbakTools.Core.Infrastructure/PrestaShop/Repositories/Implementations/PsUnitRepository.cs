using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    internal class PsUnitRepository : PsRepositoryBase<unit>, IPsUnitRepository
    {
        public PsUnitRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }

        protected override GenericFactory<unit> Factory => PrestaShopClient.UnitFactory;
    }
}
