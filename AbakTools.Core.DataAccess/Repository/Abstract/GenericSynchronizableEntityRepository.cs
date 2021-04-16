using AbakTools.Core.Domain;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class GenericSynchronizableEntityRepository<TEntity> : GenericGuidedEntityRepository<TEntity>, IGenericSynchronizableEntityRepository<TEntity>
        where TEntity : SynchronizableEntity
    {
        protected GenericSynchronizableEntityRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public override IQueryable<TEntity> GetQueryBase()
        {
            return base.GetQueryBase().Where(x => !x.IsDeleted && x.Synchronize != Framework.SynchronizeType.Deleted);
        }

        public override void Delete(TEntity entity)
        {
            entity.Delete();
            SaveOrUpdate(entity);
        }

        public TEntity GetWithArchived(int id)
        {
            return base.GetQueryBase().SingleOrDefault(x => x.Id == id);
        }
    }
}
