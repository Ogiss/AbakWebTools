using AbakTools.Core.Domain;
using AbakTools.Core.Framework.Domain;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class GenericGuidedEntityRepository<TEntity> : GenericEntityRepository<TEntity>, IGenericGuidedEntityRepository<TEntity>
        where TEntity : IGuidedEntity
    {
        protected GenericGuidedEntityRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public TEntity Get(Guid guid)
        {
            return GetQueryBase().SingleOrDefault(x => x.Guid == guid);
        }

        public async Task<TEntity> GetAsync(Guid guid)
        {
            return await GetQueryBase().SingleOrDefaultAsync(x => x.Guid == guid);
        }
    }
}
