using AbakTools.Core.Domain;
using AbakTools.Core.Framework;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class GenericBusinessEntityRepository<TEntity> : GenericGuidedEntityRepository<TEntity>
        where TEntity : BusinessEntity
    {
        protected GenericBusinessEntityRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public override IQueryable<TEntity> GetQueryBase()
        {
            return base.GetQueryBase().Where(x => x.IsDeleted == false);
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            SaveOrUpdate(entity);
        }
    }
}
