using AbakTools.Core.Domain.Tax;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class TaxRepository : GenericEntityRepository<TaxEntity>, ITaxRepository
    {
        public TaxRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
