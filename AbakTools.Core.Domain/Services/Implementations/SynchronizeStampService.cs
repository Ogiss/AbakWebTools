using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Services.Implementations
{
    class SynchronizeStampService : ISynchronizeStampService
    {
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        private readonly ISynchronizeStampRepository _synchronizeStampRepository;

        public SynchronizeStampService(IUnitOfWorkProvider unitOfWorkProvider, ISynchronizeStampRepository synchronizeStampRepository)
            => (_unitOfWorkProvider, _synchronizeStampRepository) = (unitOfWorkProvider, synchronizeStampRepository);

        public async Task<long> GetLastStampAsync(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown)
        {
            return await GetImportStampCoreAsync(code, synchronizeDirection);
        }

        public async Task SaveLastStampAsync(string code, long stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown)
        {
            await SaveImportStampCoreAsync(code, stamp, synchronizeDirection);
        }

        public async Task<DateTime> GetLastDateTimeStampAsync(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown)
        {
            return await GetDateTimeStampCoreAsync(code, synchronizeDirection);
        }

        public async Task SaveLastDateTimeStampAsync(string code, DateTime stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown)
        {
            await SaveDateTimeStampCoreAsync(code, stamp, synchronizeDirection);
        }

        private async Task<long> GetImportStampCoreAsync(string code, SynchronizeDirectionType synchronizeDirection)
        {
            using var uow = _unitOfWorkProvider.CreateReadOnly();
            return (await _synchronizeStampRepository.GetAsync(code, synchronizeDirection))?.Stamp ?? 0;
        }

        private async Task<DateTime> GetDateTimeStampCoreAsync(string code, SynchronizeDirectionType synchronizeDirection)
        {
            using var uow = _unitOfWorkProvider.CreateReadOnly();
            return (await _synchronizeStampRepository.GetAsync(code, synchronizeDirection))?.DateTimeStamp ?? new DateTime(2000, 1, 1);
        }

        private async Task SaveImportStampCoreAsync(string code, long stamp, SynchronizeDirectionType synchronizeDirection)
        {
            using (var uow = _unitOfWorkProvider.Create())
            {
                var stampInfo = _synchronizeStampRepository.GetAsync(code, synchronizeDirection).Result;

                if (stampInfo == null)
                {
                    stampInfo = new SynchronizeStampEntity(code, synchronizeDirection);
                }

                stampInfo.Stamp = stamp;

                _synchronizeStampRepository.SaveOrUpdate(stampInfo);
                uow.Commit();
            }
        }

        private async Task SaveDateTimeStampCoreAsync(string code, DateTime stamp, SynchronizeDirectionType synchronizeDirection)
        {
            using (var uow = _unitOfWorkProvider.Create())
            {
                var stampInfo = _synchronizeStampRepository.GetAsync(code, synchronizeDirection).Result;

                if (stampInfo == null)
                {
                    stampInfo = new SynchronizeStampEntity(code, synchronizeDirection);
                }

                stampInfo.DateTimeStamp = stamp;

                _synchronizeStampRepository.SaveOrUpdate(stampInfo);
                uow.Commit();
            }
        }

        public async Task<long> GetDbtsAsync()
        {
            using var uow = _unitOfWorkProvider.CreateReadOnly();
            return await _synchronizeStampRepository.GetDbtsAsync();
        }
    }
}
