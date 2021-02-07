using AbakTools.Core.Domain.Synchronize;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Services
{
    public interface ISynchronizeStampService
    {
        Task<long> GetImportStampAsync(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        Task SaveImportStampAsync(string code, long stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
    }
}
