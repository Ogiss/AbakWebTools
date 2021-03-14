using AbakTools.Core.Domain.Enova;
using AbakTools.Core.Infrastructure.Enova.Api;
using Enova.Api;
using EnovaApi.Models.DiscountGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    class EnovaCustomerDiscountRepository : IEnovaCustomerDiscountRepository
    {
        private readonly IEnovaAPiClient _api;

        public EnovaCustomerDiscountRepository(IEnovaAPiClient api) => _api = api;

        public async Task<IEnumerable<CustomerDiscountGroup>> GetModifiedCustomerDiscountsAsync(int priceDefId, long stampFrom, long stampTo)
        {
            return await _api.GetValueAsync<IEnumerable<CustomerDiscountGroup>>(
                ResourcesNames.CustomerDiscountsByStamp, $"{priceDefId}/{stampFrom}/{stampTo}");
        }
    }
}
