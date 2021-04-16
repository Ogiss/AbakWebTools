using AbakTools.Core.Domain.Enova;
using AbakTools.Core.Infrastructure.Enova.Api;
using Enova.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    class EnovaDictionaryItemRepository : IEnovaDictionaryItemRepository
    {
        private readonly IEnovaAPiClient _api;

        public EnovaDictionaryItemRepository(IEnovaAPiClient api) => _api = api;

        public async Task<IEnumerable<Guid>> GetDeletedItemsGuidsAsync(DateTime stampFrom, DateTime stampTo)
        {
            return await _api.GetValueAsync<IEnumerable<Guid>>(
                    ResourcesNames.DictionaryItemsDeleted, $"{stampFrom}/{stampTo}");
        }
    }
}
