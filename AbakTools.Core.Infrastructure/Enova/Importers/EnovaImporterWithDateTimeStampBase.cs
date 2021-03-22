using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Infrastructure.Enova.Api;
using System;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal abstract class EnovaImporterWithDateTimeStampBase<TEntry> : EnovaImporterBase<TEntry, DateTime>
    {
        protected override Task<DateTime> GetLastStamp(ISynchronizeStampService synchronizeStampService, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return synchronizeStampService.GetLastDateTimeStampAsync(code, synchronizeDirection);
        }

        protected override Task<DateTime> GetStampTo(IEnovaAPiClient api, string code, SynchronizeDirectionType synchronizeDirection)
        {
            return Task.FromResult(DateTime.Now);
        }

        protected override Task SaveLastStamp(ISynchronizeStampService synchronizeStampService, string code, DateTime stamp, SynchronizeDirectionType synchronizeDirection)
        {
            return synchronizeStampService.SaveLastDateTimeStampAsync(code, stamp, synchronizeDirection);
        }
    }
}
