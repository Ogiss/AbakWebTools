using AbakTools.Core.Framework;
using AbakTools.Core.Framework.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain
{
    public interface IGenericEntityRepository<TEntity>
        where TEntity: IEntity
    {
        IReadOnlyList<TEntity> GetList(ISpecification<TEntity> specification);
        IReadOnlyList<TResult> GetList<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);
        TEntity Get(ISpecification<TEntity> specification);
        TResult Get<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);
        Task<IReadOnlyList<TEntity>> GetListAsync(ISpecification<TEntity> specification);
        Task<IReadOnlyList<TResult>> GetListAsync<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);
        Task<TEntity> GetAsync(ISpecification<TEntity> specification);
        Task<TResult> GetAsync<TResult>(ISpecification<TEntity> specification, IProjection<TEntity, TResult> projection);
        Task<long> GetDbtsAsync();
        long GetDbts();
        TEntity Get(int id);
        void SaveOrUpdate(TEntity entity);
        Task SaveOrUpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        IList<TEntity> GetAll();
        void Flush();
    }
}
