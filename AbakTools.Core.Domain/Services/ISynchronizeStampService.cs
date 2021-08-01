using AbakTools.Core.Domain.Synchronize;
using System;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Services
{
    public interface ISynchronizeStampService
    {
        long GetDbts();
        Task<long> GetDbtsAsync();
        long GetLastStamp(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        Task<long> GetLastStampAsync(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        void SaveLastStamp(string code, long stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        Task SaveLastStampAsync(string code, long stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        DateTime GetLastDateTimeStamp(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        Task<DateTime> GetLastDateTimeStampAsync(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        void SaveLastDateTimeStamp(string code, DateTime stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
        Task SaveLastDateTimeStampAsync(string code, DateTime stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown);
    }
}
