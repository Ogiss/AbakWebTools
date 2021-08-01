using AbakTools.Core.Domain.Services;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    class UnitExporter : PrestaShopExporterBase<int>
    {
        private readonly IUnitSynchronizeService _unitSynchronizeService;

        public UnitExporter(
            ILogger<UnitExporter> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampService synchronizeStampService,
            IUnitSynchronizeService unitSynchronizeService)
            : base(logger, unitOfWorkProvider, synchronizeStampService)
            => _unitSynchronizeService = unitSynchronizeService;

        protected override IEnumerable<int> GetExportingEntries(CancellationToken cancelerationToken)
        {
            using (var uow = UnitOfWorkProvider.CreateReadOnly())
            {
                return _unitSynchronizeService.GetUnitsIdsToSynchonise(StampFrom, StampTo);
            }
        }

        protected override void ProcessEntry(int id)
        {
            using var uow = UnitOfWorkProvider.Create();
            _unitSynchronizeService.Synchronize(id);
            uow.Commit();
        }
    }
}
