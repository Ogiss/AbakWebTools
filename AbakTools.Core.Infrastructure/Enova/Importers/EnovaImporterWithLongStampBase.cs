using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Infrastructure.Enova.Api;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal abstract class EnovaImporterWithLongStampBase<TEntry> : EnovaImporterBase<TEntry, long>
    {
        protected override Task<long> GetLastStamp(ISynchronizeStampService synchronizeStampService, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return synchronizeStampService.GetLastStampAsync(SynchronizeCode, SynchronizeDirection);
        }

        protected override Task<long> GetStampTo(IEnovaAPiClient api, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return api.GetDbtsAsync();
        }

        protected override Task SaveLastStamp(ISynchronizeStampService synchronizeStampService, string code, long stamp, SynchronizeDirectionType synchronizeDirection)
        {
            return synchronizeStampService.SaveLastStampAsync(SynchronizeCode, stamp, SynchronizeDirection);
        }
    }
}
