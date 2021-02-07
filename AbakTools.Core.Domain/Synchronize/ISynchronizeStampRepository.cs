using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Synchronize
{
    public interface ISynchronizeStampRepository : IGenericEntityRepository<SynchronizeStampEntity>
    {
        SynchronizeStampEntity Get(string code, SynchronizeDirectionType type);
        Task<SynchronizeStampEntity> GetAsync(string code, SynchronizeDirectionType type, CancellationToken cancellationToken = default);
    }
}
