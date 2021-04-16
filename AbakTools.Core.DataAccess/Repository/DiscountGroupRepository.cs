using AbakTools.Core.Domain.DiscountGroup;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using AbakTools.Core.Framework;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class DiscountGroupRepository : GenericSynchronizableEntityRepository<DiscountGroupEntity>, IDiscountGroupRepository
    {
        public DiscountGroupRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public async Task<IEnumerable<int>> GetNewOrModifiedGroupsIdsAsync(long stampFrom, long stampTo, CancellationToken cancellationToken = default)
        {
            return await CurrentSession.CreateQuery(
                $"SELECT {nameof(DiscountGroupEntity.Id)} " +
                $"FROM {nameof(DiscountGroupEntity)} " +
                $"WHERE {nameof(DiscountGroupEntity.Synchronize)} <> {(int)SynchronizeType.Synchronized} AND CONVERT(BIGINT, Stamp) > {stampFrom} AND CONVERT(BIGINT, Stamp) <= {stampTo}")
                .ListAsync<int>(cancellationToken);
        }
    }
}
