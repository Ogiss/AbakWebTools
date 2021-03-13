using AbakTools.Core.Domain.DiscountGroup;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class DiscountGroupRepository : GenericGuidedEntityRepository<DiscountGroupEntity>, IDiscountGroupRepository
    {
        public DiscountGroupRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
