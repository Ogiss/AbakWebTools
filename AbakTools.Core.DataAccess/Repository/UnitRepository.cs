using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class UnitRepository : GenericGuidedEntityRepository<UnitEntity>, IUnitRepository
    {
        public UnitRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
