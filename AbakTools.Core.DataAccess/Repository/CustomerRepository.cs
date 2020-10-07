using AbakTools.Core.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class CustomerRepository : GenericBusinessEntityRepository<CustomerEntity>, ICustomerRepository
    {
        public CustomerRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public CustomerEntity GetByWebId(int webId)
        {
            return GetQueryBase().SingleOrDefault(x => x.WebId == webId);
        }

        public IReadOnlyCollection<int> GetModifiedCustomersIds(DateTime stampFrom, DateTime? stampTo)
        {
            var query = GetQueryBase().Where(x => x.ModificationDate > stampFrom);

            if (stampTo.HasValue)
            {
                query = query.Where(x => x.ModificationDate <= stampTo);
            }

            return query.Select(x => x.Id).ToList();
        }

        public IReadOnlyCollection<int> GetModifiedCustomersIdsWithWebAccount(DateTime stampFrom, DateTime? stampTo)
        {
            var query = GetQueryBase().Where(x => x.ModificationDate > stampFrom && x.WebAccountLogin != null && x.WebAccountLogin != "");

            if (stampTo.HasValue)
            {
                query = query.Where(x => x.ModificationDate <= stampTo);
            }

            return query.Select(x => x.Id).ToList();
        }
    }
}
