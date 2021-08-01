using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Infrastructure.Enova.Api;
using System;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal abstract class EnovaImporterWithDateTimeStampBase<TEntry> : EnovaImporterBase<TEntry, DateTime>
    {
        protected override DateTime GetLastStamp(ISynchronizeStampService synchronizeStampService, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return synchronizeStampService.GetLastDateTimeStamp(code, synchronizeDirection);
        }

        protected override DateTime GetStampTo(IEnovaAPiClient api, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return DateTime.Now;
        }

        protected override void SaveLastStamp(ISynchronizeStampService synchronizeStampService, string code, DateTime stamp, SynchronizeDirectionType synchronizeDirection)
        {
            synchronizeStampService.SaveLastDateTimeStamp(code, stamp, synchronizeDirection);
        }
    }
}
