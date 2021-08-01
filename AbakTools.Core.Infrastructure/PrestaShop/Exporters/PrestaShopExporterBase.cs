using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    abstract class PrestaShopExporterBase<TEntry> : IPrestaShopExporter
    {
        private ISynchronizeStampService _synchronizeStampService;

        protected ILogger Logger { get; }
        protected IUnitOfWorkProvider UnitOfWorkProvider { get; }
        protected long StampFrom { get; private set; }
        protected long StampTo { get; private set; }

        protected PrestaShopExporterBase(ILogger logger, IUnitOfWorkProvider unitOfWorkProvider, ISynchronizeStampService synchronizeStampService)
            => (Logger, UnitOfWorkProvider, _synchronizeStampService) = (logger, unitOfWorkProvider, synchronizeStampService);

        public void StartExport(CancellationToken cancelerationToken)
        {
            StampFrom = _synchronizeStampService.GetLastStamp(GetType().Name);
            StampTo = _synchronizeStampService.GetDbts();

            if (StampFrom < StampTo)
            {
                var entries = GetExportingEntries(cancelerationToken);

                if (entries?.Any() ?? false)
                {
                    Logger.LogDebug($"[{GetType().Name}] Starting export {entries.Count()} entries.");

                    var startDt = DateTime.Now;
                    var parallelOptions = new ParallelOptions
                    {
                        CancellationToken = cancelerationToken,
#if DEBUG
                        MaxDegreeOfParallelism = 1
#else
                        MaxDegreeOfParallelism = 2
#endif
                    };

                    Parallel.ForEach(entries, parallelOptions, ProcessEntryCore);

                    _synchronizeStampService.SaveLastStamp(GetType().Name, StampTo);

                    Logger.LogDebug($"[{GetType().Name}] Export entries finished in {(DateTime.Now - startDt).TotalSeconds} sec.");
                }
            }
        }

        protected virtual void ProcessEntryCore(TEntry entry)
        {
            try
            {
                ProcessEntry(entry);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        protected abstract IEnumerable<TEntry> GetExportingEntries(CancellationToken cancelerationToken);
        protected abstract void ProcessEntry(TEntry entry);
    }
}
