using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.DiscountGroup
{
    public interface IDiscountGroupRepository : IGenericSynchronizableEntityRepository<DiscountGroupEntity>
    {
        Task<IEnumerable<int>> GetNewOrModifiedGroupsIdsAsync(long stampFrom, long stampTo, CancellationToken cancellationToken = default);
    }
}
