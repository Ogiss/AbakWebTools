using AbakTools.Core.Domain.Enova.Customer;
using AbakTools.Core.Infrastructure.Enova.Api;
using Enova.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    internal class EnovaCustomerRepository : EnovaGenericGuidedEntityRepository<EnovaCustomer>, IEnovaCustomerRepository
    {
        protected override string Resource => ResourcesNames.Customers;

        public EnovaCustomerRepository(IEnovaAPiClient enovaAPiClient) : base(enovaAPiClient)
        {
        }
    }
}
