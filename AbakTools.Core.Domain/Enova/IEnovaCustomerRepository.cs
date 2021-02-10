using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api = EnovaApi.Models.Customer;

namespace AbakTools.Core.Domain.Enova
{
    public interface IEnovaCustomerRepository : IEnovaGenericRepository<Api.Customer>
    {
        Task<IEnumerable<Guid>> GetModifiedCustomersGuidsAsync(long stampFrom, long stampTo);
    }
}
