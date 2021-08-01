using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.DiscountGroup
{
    public interface IDiscountGroupRepository : IGenericSynchronizableEntityRepository<DiscountGroupEntity>
    {
        IEnumerable<int> GetNewOrModifiedGroupsIds(long stampFrom, long stampTo);
    }
}
