using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.Enova.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal abstract class EnovaImporterBase<TEntry, TStamp> : IEnovaImporter
    {
        private ISynchronizeStampService _synchronizeStampService;
        private IEnovaAPiClient _api;
        private TStamp _stampFrom;
        private TStamp _stampTo;

        protected IUnitOfWorkProvider UnitOfWorkProvider;
        protected TStamp StampFrom => _stampFrom;
        protected TStamp StampTo => _stampTo;
        protected virtual string SynchronizeCode => GetType().Name;
        protected virtual SynchronizeDirectionType SynchronizeDirection => SynchronizeDirectionType.Unknown;
        protected abstract ILogger Logger { get; }

        public Task RunImport(IServiceScope serviceScope, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogDebug($"Starting enova import processing: {this.GetType().Name}");

                UnitOfWorkProvider = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkProvider>();
                _synchronizeStampService = serviceScope.ServiceProvider.GetRequiredService<ISynchronizeStampService>();
                _api = serviceScope.ServiceProvider.GetRequiredService<IEnovaAPiClient>();

                _stampFrom = GetLastStamp(_synchronizeStampService, SynchronizeCode, SynchronizeDirection);
                _stampTo = GetStampTo(_api, SynchronizeCode, SynchronizeDirection);

                IEnumerable<TEntry> entries;

                entries = GetEntries(_stampFrom, _stampTo);

                if (entries.Any())
                {
                    var startDt = DateTime.Now;

                    Logger.LogDebug($"[{GetType().Name}]: Starting import");

                    Parallel.ForEach(entries, GetParrallelOptions(), ProcessEntryCore);

                    SaveLastStamp(_synchronizeStampService, SynchronizeCode, _stampTo, SynchronizeDirection);
                    Logger.LogDebug($"[{GetType().Name}]: Import finished in {(DateTime.Now - startDt).TotalSeconds} sec.");
                }
            }
            catch (Exception ex)
            {
                HandleImportExeception(ex);
            }

            return Task.CompletedTask;
        }

        private void ProcessEntryCore(TEntry entry)
        {
            try
            {
                //using var uow = _unitOfWorkProvider.Create();
                ProcessEntry(entry);
                //uow.Commit();
            }
            catch (Exception ex)
            {
                HandleProcessEntryException(entry, ex);
            }
        }

        protected virtual void HandleImportExeception(Exception exception)
        {
        }

        protected virtual void HandleProcessEntryException(TEntry entry, Exception exception)
        {
        }

        protected virtual ParallelOptions GetParrallelOptions()
        {
            return new ParallelOptions
            {
#if DEBUG
                MaxDegreeOfParallelism = 1
#else
                MaxDegreeOfParallelism = 2
#endif

            };
        }

        protected abstract void ProcessEntry(TEntry entry);
        protected abstract IEnumerable<TEntry> GetEntries(TStamp stampFrom, TStamp stampTo);
        protected abstract TStamp GetLastStamp(ISynchronizeStampService synchronizeStampService, string code, SynchronizeDirectionType synchronizeDirection);
        protected abstract TStamp GetStampTo(IEnovaAPiClient api, string code, SynchronizeDirectionType synchronizeDirection);
        protected abstract void SaveLastStamp(ISynchronizeStampService synchronizeStampService, string code, TStamp stamp, SynchronizeDirectionType synchronizeDirection);
    }
}
