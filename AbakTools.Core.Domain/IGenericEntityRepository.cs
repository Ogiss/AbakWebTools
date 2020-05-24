using AbakTools.Core.Framework;
using System.Collections.Generic;

namespace AbakTools.Core.Domain
{
    public interface IGenericEntityRepository<TEntity>
        where TEntity: IEntity
    {
        long GetDbts();
        TEntity Get(int id);
        void SaveOrUpdate(TEntity entity);
        void Delete(TEntity entity);
        IList<TEntity> GetAll();
        void Flush();
    }
}
