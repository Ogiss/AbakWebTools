using AbakTools.Core.Domain.Customer;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace AbakTools.Core.DataAccess.Repository
{
    class CustomerDiscountGroupRepository : GenericGuidedEntityRepository<CustomerDiscountEntity>, ICustomerDiscountGroupRepository
    {
        public CustomerDiscountGroupRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        /*
        public async Task<IEnumerable<int>> GetNewOrModifiedCustomerDiscountIds(long stampFrom, long stampTo)
        {
            return await GetQueryBase()
                .Where(x => x.Synchronize != Framework.SynchronizeType.Synchronized && x.Stamp > stampFrom && x.Stamp <= stampTo)
                .Select(x => x.Id)
                .ToListAsync();
        }
        */
    }
}
