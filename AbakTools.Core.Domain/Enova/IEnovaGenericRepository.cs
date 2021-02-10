using System;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Enova
{
    public interface IEnovaGenericRepository<TEntity>
    {
        TEntity Get(Guid guid);
        Task<TEntity> GetAsync(Guid guid);
    }
}
