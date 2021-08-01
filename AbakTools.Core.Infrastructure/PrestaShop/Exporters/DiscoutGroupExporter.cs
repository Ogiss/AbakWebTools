using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using AbakTools.Core.Infrastructure.PrestaShop.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PsDiscountGroup = Bukimedia.PrestaSharp.Entities.discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    class DiscoutGroupExporter : PrestaShopExporterBase<int>
    {
        private readonly IDiscountGroupRepository _discountGroupRepository;
        private readonly IPsDiscountGroupRepository _psDiscountGroupRepository;
        private readonly IDiscountGroupSynchronizeService _discountGroupSynchronizeService;

        public DiscoutGroupExporter(
            ILogger<DiscoutGroupExporter> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampService synchronizeStampService,
            IDiscountGroupRepository discountGroupRepository,
            IPsDiscountGroupRepository psDiscountGroupRepository,
            IDiscountGroupSynchronizeService discountGroupSynchronizeService) : base(logger, unitOfWorkProvider, synchronizeStampService)
        {
            _discountGroupRepository = discountGroupRepository;
            _psDiscountGroupRepository = psDiscountGroupRepository;
            _discountGroupSynchronizeService = discountGroupSynchronizeService;
        }

        protected override IEnumerable<int> GetExportingEntries(CancellationToken cancelerationToken)
        {
            using (UnitOfWorkProvider.CreateReadOnly())
            {
                return _discountGroupRepository.GetNewOrModifiedGroupsIds(StampFrom, StampTo);
            }
        }

        protected override void ProcessEntry(int id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var discountGroup = _discountGroupRepository.GetWithArchived(id);

                if (discountGroup != null && discountGroup.Stamp <= StampTo)
                {
                    _discountGroupSynchronizeService.Synchronize(discountGroup);
                    uow.Commit();
                }
            }
        }
    }
}
