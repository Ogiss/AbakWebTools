using AbakTools.Core.Domain.Supplier;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class SupplierRepository : GenericGuidedEntityRepository<SupplierEntity>, ISupplierRepository
    {
        public SupplierRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public IReadOnlyCollection<SupplierEntity> GetAllModified(long stampFrom, long stampTo)
        {
            return GetQueryBase().Where(x => x.Stamp > stampFrom && x.Stamp <= stampTo).ToList();
        }
    }
}
