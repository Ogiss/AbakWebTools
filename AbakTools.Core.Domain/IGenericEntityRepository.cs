using AbakTools.Core.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain
{
    public interface IGenericEntityRepository<TEntity>
        where TEntity: IEntity
    {
        IReadOnlyList<TEntity> GetList(ISpecyfication<TEntity> specification);
        IReadOnlyList<int> GetIds(ISpecyfication<TEntity> specification);
        TEntity Get(ISpecyfication<TEntity> specification);
        int? GetId(ISpecyfication<TEntity> specification);
        Task<IReadOnlyList<TEntity>> GetListAsync(ISpecyfication<TEntity> specification);
        Task<IReadOnlyList<int>> GetIdsAsync(ISpecyfication<TEntity> specification);
        Task<TEntity> GetAsync(ISpecyfication<TEntity> specification);
        Task<int?> GetIdAsync(ISpecyfication<TEntity> specification);
        Task<long> GetDbtsAsync();
        TEntity Get(int id);
        void SaveOrUpdate(TEntity entity);
        Task SaveOrUpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        IList<TEntity> GetAll();
        void Flush();
    }
}
