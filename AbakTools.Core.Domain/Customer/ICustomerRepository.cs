using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain.Customer
{
    public interface ICustomerRepository : IGenericGuidedEntityRepository<CustomerEntity>
    {
        IReadOnlyCollection<int> GetModifiedCustomersIds(DateTime stampFrom, DateTime? stampTo);
        IReadOnlyCollection<int> GetModifiedCustomersIdsWithWebAccount(DateTime stampFrom, DateTime? stampTo);
    }
}
