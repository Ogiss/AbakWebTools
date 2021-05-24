using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Customer
{
    public interface ICustomerDiscountGroupRepository : IGenericGuidedEntityRepository<CustomerDiscountEntity>
    {
       // Task<IEnumerable<int>> GetNewOrModifiedCustomerDiscountIds(long stampFrom, long stampTo);
    }
}
