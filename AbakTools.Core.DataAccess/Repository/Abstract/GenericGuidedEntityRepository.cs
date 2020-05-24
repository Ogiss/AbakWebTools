using AbakTools.Core.Domain;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
