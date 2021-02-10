using AbakTools.Core.Domain;
using AbakTools.Core.Domain.Enova;
using AbakTools.Core.Infrastructure.Enova.Api;
using System;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    internal abstract class EnovaGenericGuidedEntityRepository<TEntity> : EnovaGenericEntityRepository<TEntity>, IEnovaGenericRepository<TEntity>
        where TEntity : EnovaApi.Models.Common.GuidedEntity
    {
        protected EnovaGenericGuidedEntityRepository(IEnovaAPiClient enovaAPiClient) : base(enovaAPiClient)
        {
        }

        public TEntity Get(Guid guid)
        {
            return GetAsync(guid).Result;
        }

        public async Task<TEntity> GetAsync(Guid guid)
        {
            return await Api.GetValueAsync<TEntity>(Resource, guid);
        }
    }
}
