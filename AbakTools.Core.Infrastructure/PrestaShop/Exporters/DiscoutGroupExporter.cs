using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Services;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
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
        private readonly IPSDiscountGroupRepository _psDiscountGroupRepository;

        public DiscoutGroupExporter(
            ILogger<DiscoutGroupExporter> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampService synchronizeStampService,
            IDiscountGroupRepository discountGroupRepository,
            IPSDiscountGroupRepository psDiscountGroupRepository) : base(logger, unitOfWorkProvider, synchronizeStampService)
        {
            _discountGroupRepository = discountGroupRepository;
            _psDiscountGroupRepository = psDiscountGroupRepository;
        }

        protected override async Task<IEnumerable<int>> GetExportingEntriesAsync(CancellationToken cancelerationToken)
        {
            using (UnitOfWorkProvider.CreateReadOnly())
            {
                return await _discountGroupRepository.GetNewOrModifiedGroupsIdsAsync(StampFrom, StampTo, cancelerationToken);
            }
        }

        protected override void ProcessEntry(int id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var discountGroup = _discountGroupRepository.GetWithArchived(id);

                if (discountGroup != null && discountGroup.Stamp <= StampTo)
                {
                    var psDiscountGroup = GetPsDiscountGroup(discountGroup);

                    if (discountGroup.IsArchived)
                    {
                        DeleteDiscountGroup(discountGroup, psDiscountGroup);
                    }
                    else
                    {
                        if (psDiscountGroup == null)
                        {
                            psDiscountGroup = InsertDiscountGroup(discountGroup);
                        }
                        else
                        {
                            UpdateDiscountGroup(discountGroup, psDiscountGroup);
                        }

                        discountGroup.SetWebId((int)psDiscountGroup.id);
                    }

                    discountGroup.MakeSynchronized();
                    _discountGroupRepository.SaveOrUpdate(discountGroup);
                    uow.Commit();
                }
            }
        }

        private PsDiscountGroup GetPsDiscountGroup(DiscountGroupEntity discountGroup)
        {
            if (discountGroup.WebId.HasValue)
            {
                return _psDiscountGroupRepository.Get(discountGroup.WebId.Value);
            }

            return default;
        }

        private PsDiscountGroup InsertDiscountGroup(DiscountGroupEntity discountGroup)
        {
            Logger.LogDebug($"Insert discount group {discountGroup.Name} ({discountGroup.Category}).");

            var psDiscountGroup = new PsDiscountGroup
            {
                category = discountGroup.Category,
                name = discountGroup.Name
            };

            return _psDiscountGroupRepository.SaveOrUpdate(psDiscountGroup);
        }

        private void UpdateDiscountGroup(DiscountGroupEntity discountGroup, PsDiscountGroup psDiscountGroup)
        {
            if (!Equals(discountGroup.Name, psDiscountGroup.name) || !Equals(discountGroup.Category, psDiscountGroup.category))
            {
                Logger.LogDebug($"Updating discount group {discountGroup.Name} ({discountGroup.Category}).");

                psDiscountGroup.name = discountGroup.Name;
                psDiscountGroup.category = discountGroup.Category;
                _psDiscountGroupRepository.SaveOrUpdate(psDiscountGroup);
            }
        }

        private void DeleteDiscountGroup(DiscountGroupEntity discountGroup, PsDiscountGroup psDiscountGroup)
        {
            if (psDiscountGroup != null && psDiscountGroup.deleted == 0)
            {
                Logger.LogDebug($"Deleting discount group {discountGroup.Name} ({discountGroup.Category}).");

                psDiscountGroup.deleted = 1;
                _psDiscountGroupRepository.SaveOrUpdate(psDiscountGroup);
            }
        }
    }
}
