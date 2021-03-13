using AbakTools.Core.Domain.Enova;
using AbakTools.Core.Infrastructure.Enova.Api;
using Enova.Api;
using EnovaApi.Models.DiscountGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    class EnovaDiscountGroupRepository : IEnovaDiscountGroupRepository
    {
        private readonly IEnovaAPiClient _api;

        public EnovaDiscountGroupRepository(IEnovaAPiClient api) => _api = api;

        public async Task<IEnumerable<DiscountGroup>> GetModifiedDiscountGroupAsync(int priceDefId, long stampFrom, long stampTo)
        {
            return await _api.GetValueAsync<IEnumerable<DiscountGroup>>(
                ResourcesNames.DiscountGroupsByStamp, $"{priceDefId}/{stampFrom}/{stampTo}");
        }
    }
}
