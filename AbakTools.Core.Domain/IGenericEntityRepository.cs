using AbakTools.Core.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain
{
    public interface IGenericEntityRepository<TEntity>
        where TEntity: IEntity
    {
        IReadOnlyList<TEntity> GetList(ISpecification<TEntity> specification);
        IReadOnlyList<int> GetIds(ISpecification<TEntity> specification);
        TEntity Get(ISpecification<TEntity> specification);
        int? GetId(ISpecification<TEntity> specification);
        Task<IReadOnlyList<TEntity>> GetListAsync(ISpecification<TEntity> specification);
        Task<IReadOnlyList<int>> GetIdsAsync(ISpecification<TEntity> specification);
        Task<TEntity> GetAsync(ISpecification<TEntity> specification);
        Task<int?> GetIdAsync(ISpecification<TEntity> specification);
        Task<long> GetDbtsAsync();
        TEntity Get(int id);
        void SaveOrUpdate(TEntity entity);
        Task SaveOrUpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        IList<TEntity> GetAll();
        void Flush();
    }
}
