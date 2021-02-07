using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Services.Implementations
{
    class SynchronizeStampService : ISynchronizeStampService
    {
        private readonly IUnitOfWorkProvider _unitOfWorkProvider;
        private readonly ISynchronizeStampRepository _synchronizeStampRepository;

        public SynchronizeStampService(IUnitOfWorkProvider unitOfWorkProvider, ISynchronizeStampRepository synchronizeStampRepository)
            => (_unitOfWorkProvider, _synchronizeStampRepository) = (unitOfWorkProvider, synchronizeStampRepository);

        public async Task<long> GetImportStampAsync(string code, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown)
        {
            return await GetImportStampCoreAsync(code, synchronizeDirection);
        }

        public async Task SaveImportStampAsync(string code, long stamp, SynchronizeDirectionType synchronizeDirection = SynchronizeDirectionType.Unknown)
        {
            await SaveImportStampCoreAsync(code, stamp, synchronizeDirection);
        }

        private async Task<long> GetImportStampCoreAsync(string code, SynchronizeDirectionType synchronizeDirection)
        {
            using var uow = _unitOfWorkProvider.CreateReadOnly();
            return (await _synchronizeStampRepository.GetAsync(code, synchronizeDirection))?.Stamp ?? 0;
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
    }
}
