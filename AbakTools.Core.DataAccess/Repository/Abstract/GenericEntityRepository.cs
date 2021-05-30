using AbakTools.Core.Domain;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;

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

        public IReadOnlyList<TEntity> GetList(ISpecification<TEntity> specification)
        {
            return GetQueryBase().Where(specification.ToExpression()).ToList();
        }

        public IReadOnlyList<int> GetIds(ISpecification<TEntity> specification)
        {
            return GetQueryBase().Where(specification.ToExpression()).Select(x => x.Id).ToList();
        }

        public TEntity Get(ISpecification<TEntity> specification)
        {
            return GetQueryBase().SingleOrDefault(specification.ToExpression());
        }

        public int? GetId(ISpecification<TEntity> specification)
        {
            return GetQueryBase().SingleOrDefault(specification.ToExpression())?.Id;
        }

        public async Task<IReadOnlyList<TEntity>> GetListAsync(ISpecification<TEntity> specification)
        {
            return await GetQueryBase().Where(specification.ToExpression()).ToListAsync();
        }

        public async Task<IReadOnlyList<int>> GetIdsAsync(ISpecification<TEntity> specification)
        {
            return await GetQueryBase().Where(specification.ToExpression()).Select(x => x.Id).ToListAsync();
        }

        public async Task<TEntity> GetAsync(ISpecification<TEntity> specification)
        {
            return await GetQueryBase().SingleOrDefaultAsync(specification.ToExpression());
        }

        public async Task<int?> GetIdAsync(ISpecification<TEntity> specification)
        {
            return await GetQueryBase().Where(specification.ToExpression()).Select(x => x.Id).SingleOrDefaultAsync();
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
