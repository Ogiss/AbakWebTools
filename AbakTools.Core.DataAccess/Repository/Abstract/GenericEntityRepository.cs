using AbakTools.Core.Domain;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class GenericEntityRepository<TEntity> : BaseRepository, IGenericEntityRepository<TEntity>
        where TEntity : IEntity
    {
        protected GenericEntityRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public virtual IQueryable<TEntity> GetQueryBase()
        {
            return CurrentSession.Query<TEntity>();
        }


        public virtual void Delete(TEntity entity)
        {
            CurrentSession.Delete(entity);
        }

        public virtual TEntity Get(int id)
        {
            return GetQueryBase().SingleOrDefault(x => x.Id == id);
        }

        public virtual IList<TEntity> GetAll()
        {
            return GetQueryBase().ToList();
        }

        public virtual void SaveOrUpdate(TEntity entity)
        {
            CurrentSession.SaveOrUpdate(entity);
        }

        public async Task SaveOrUpdateAsync(TEntity entity)
        {
            await CurrentSession.SaveOrUpdateAsync(entity);
        }
    }
}
