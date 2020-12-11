using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
