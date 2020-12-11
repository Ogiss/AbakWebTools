using AbakTools.Core.Framework;
using System;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain
{
    public interface IGenericGuidedEntityRepository<TEntity> : IGenericEntityRepository<TEntity>
        where TEntity: IGuidedEntity
    {
        TEntity Get(Guid guid);
        Task<TEntity> GetAsync(Guid guid);
    }
}
