using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.Enova.Api;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    internal abstract class EnovaImporterBase<TEntry, TStamp> : IEnovaImporter
    {
        private IUnitOfWorkProvider _unitOfWorkProvider;
        private ISynchronizeStampService _synchronizeStampService;
        private IEnovaAPiClient _api;
        private TStamp _stampFrom;
        private TStamp _stampTo;

        protected TStamp StampFrom => _stampFrom;
        protected TStamp StampTo => _stampTo;
        protected virtual string SynchronizeCode => GetType().Name;
        protected virtual SynchronizeDirectionType SynchronizeDirection => SynchronizeDirectionType.Unknown;

        public Task RunImport(IServiceScope serviceScope)
        {
            try
            {
                _unitOfWorkProvider = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkProvider>();
                _synchronizeStampService = serviceScope.ServiceProvider.GetRequiredService<ISynchronizeStampService>();
                _api = serviceScope.ServiceProvider.GetRequiredService<IEnovaAPiClient>();

                //_stampFrom = _synchronizeStampService.GetLastStampAsync(SynchronizeCode, SynchronizeDirection).Result;
                //_stampTo = _api.GetDbtsAsync().Result;
                _stampFrom = GetLastStamp(_synchronizeStampService, SynchronizeCode, SynchronizeDirection).Result;
                _stampTo = GetStampTo(_api, SynchronizeCode, SynchronizeDirection).Result;

                IEnumerable<TEntry> entries;

                using (var uow = _unitOfWorkProvider.CreateReadOnly())
                {
                    entries = GetEntriesAsync(_stampFrom, _stampTo).Result;
                }

                if (entries.Any())
                {

                    Parallel.ForEach(entries, GetParrallelOptions(), ProcessEntryCore);
                }

                //_synchronizeStampService.SaveImportStampAsync(SynchronizeCode, _stampTo, SynchronizeDirection).Wait();
                SaveLastStamp(_synchronizeStampService, SynchronizeCode, _stampTo, SynchronizeDirection).Wait();
            }
            catch(Exception ex)
            {
                HandleImportExeception(ex);
            }

            return Task.CompletedTask;
        }

        private void ProcessEntryCore(TEntry entry)
        {
            try
            {
                using var uow = _unitOfWorkProvider.Create();
                ProcessEntry(entry);
                uow.Commit();
            }
            catch(Exception ex)
            {
                HandleProcessEntryException(entry, ex);
            }
        }

        protected abstract void ProcessEntry(TEntry entry);

        //protected abstract Task<IEnumerable<TEntry>> GetEntriesAsync(long stampFrom, long stampTo);
        protected abstract Task<IEnumerable<TEntry>> GetEntriesAsync(TStamp stampFrom, TStamp stampTo);

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

        protected abstract Task<TStamp> GetLastStamp(ISynchronizeStampService synchronizeStampService, string code, SynchronizeDirectionType synchronizeDirection);
        protected abstract Task<TStamp> GetStampTo(IEnovaAPiClient api, string code, SynchronizeDirectionType synchronizeDirection);
        protected abstract Task SaveLastStamp(ISynchronizeStampService synchronizeStampService, string code, TStamp stamp, SynchronizeDirectionType synchronizeDirection);
    }
}
