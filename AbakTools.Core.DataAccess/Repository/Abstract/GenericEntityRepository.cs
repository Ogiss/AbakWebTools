using AbakTools.Core.Domain;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Linq;
using AbakTools.Core.Framework.Domain;

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

        protected virtual IQueryable<TEntity> GetQuery(ISpecification<TEntity> specification)
        {
            return GetQueryBase().Where(specification.ToExpression());
        }

        public IReadOnlyList<TEntity> GetList(ISpecification<TEntity> specification)
        {
            return GetQuery(specification).ToList();
        }

        public IReadOnlyList<TResult> GetList<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection)
        {
            return GetQuery(specification).Select(projection.ToExpression()).ToList();
        }

        public TEntity Get(ISpecification<TEntity> specification)
        {
            return GetQuery(specification).SingleOrDefault();
        }

        public TResult Get<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection)
        {
            return GetQuery(specification).Select(projection.ToExpression()).SingleOrDefault();
        }

        public async Task<IReadOnlyList<TEntity>> GetListAsync(ISpecification<TEntity> specification)
        {
            return await GetQuery(specification).ToListAsync();
        }

        public async Task<IReadOnlyList<TResult>> GetListAsync<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection)
        {
            return await GetQuery(specification).Select(projection.ToExpression()).ToListAsync();
        }

        public async Task<TEntity> GetAsync(ISpecification<TEntity> specification)
        {
            return await GetQuery(specification).SingleOrDefaultAsync();
        }

        public async Task<TResult> GetAsync<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection)
        {
            return await GetQuery(specification).Select(projection.ToExpression()).SingleOrDefaultAsync();
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
