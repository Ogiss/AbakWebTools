using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Infrastructure.Enova.Api;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal abstract class EnovaImporterWithLongStampBase<TEntry> : EnovaImporterBase<TEntry, long>
    {
        protected override long GetLastStamp(ISynchronizeStampService synchronizeStampService, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return synchronizeStampService.GetLastStamp(SynchronizeCode, SynchronizeDirection);
        }

        protected override long GetStampTo(IEnovaAPiClient api, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return api.GetDbtsAsync().Result;
        }

        protected override void SaveLastStamp(ISynchronizeStampService synchronizeStampService, string code, long stamp, SynchronizeDirectionType synchronizeDirection)
        {
            synchronizeStampService.SaveLastStamp(SynchronizeCode, stamp, SynchronizeDirection);
        }
    }
}
