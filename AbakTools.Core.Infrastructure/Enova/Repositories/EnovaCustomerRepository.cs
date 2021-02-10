using AbakTools.Core.Domain.Enova;
using AbakTools.Core.Infrastructure.Enova.Api;
using Enova.Api;
using EnovaApi.Models.Customer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    internal class EnovaCustomerRepository : EnovaGenericGuidedEntityRepository<Customer>, IEnovaCustomerRepository
    {
        protected override string Resource => ResourcesNames.Customers;

        public EnovaCustomerRepository(IEnovaAPiClient enovaAPiClient) : base(enovaAPiClient)
        {
        }

        public async Task<IEnumerable<Guid>> GetModifiedCustomersGuidsAsync(long stampFrom, long stampTo)
        {
            return await Api.GetValueAsync<IEnumerable<Guid>>(ResourcesNames.CustomersByStamp, $"{stampFrom}/{stampTo}");
        }
    }
}
