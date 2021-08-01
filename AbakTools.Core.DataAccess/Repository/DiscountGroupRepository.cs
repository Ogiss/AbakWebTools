using AbakTools.Core.Domain.DiscountGroup;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class DiscountGroupRepository : GenericSynchronizableEntityRepository<DiscountGroupEntity>, IDiscountGroupRepository
    {
        public DiscountGroupRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public IEnumerable<int> GetNewOrModifiedGroupsIds(long stampFrom, long stampTo)
        {
            return CurrentSession.CreateQuery(
                $"SELECT {nameof(DiscountGroupEntity.Id)} " +
                $"FROM {nameof(DiscountGroupEntity)} " +
                $"WHERE {nameof(DiscountGroupEntity.Synchronize)} <> {(int)SynchronizeType.Synchronized} AND CONVERT(BIGINT, Stamp) > {stampFrom} AND CONVERT(BIGINT, Stamp) <= {stampTo}")
                .List<int>();
        }
    }
}
