using AbakTools.Core.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain
{
    public interface IGenericEntityRepository<TEntity>
        where TEntity: IEntity
    {
        Task<long> GetDbtsAsync();
        TEntity Get(int id);
        void SaveOrUpdate(TEntity entity);
        Task SaveOrUpdateAsync(TEntity entity);
        void Delete(TEntity entity);
        IList<TEntity> GetAll();
        void Flush();
    }
}
