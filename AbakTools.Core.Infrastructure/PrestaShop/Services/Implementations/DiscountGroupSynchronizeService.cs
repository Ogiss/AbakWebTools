using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Bukimedia.PrestaSharp.Entities;
using Microsoft.Extensions.Logging;
using PsDiscountGroup = Bukimedia.PrestaSharp.Entities.discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services.Implementations
{
    class DiscountGroupSynchronizeService : IDiscountGroupSynchronizeService
    {
        private readonly ILogger _logger;
        private readonly IDiscountGroupRepository _discountGroupRepository;
        private readonly IPsDiscountGroupRepository _psDiscountGroupRepository;

        public DiscountGroupSynchronizeService(
            ILogger<DiscountGroupSynchronizeService> logger, IDiscountGroupRepository discountGroupRepository, IPsDiscountGroupRepository psDiscountGroupRepository)
            => (_logger, _discountGroupRepository, _psDiscountGroupRepository) = (logger, discountGroupRepository, psDiscountGroupRepository);

        public PsDiscountGroup Synchronize(DiscountGroupEntity discountGroup)
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

            return psDiscountGroup;
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
            _logger.LogDebug($"Insert discount group {discountGroup.Name} ({discountGroup.Category}).");

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
                _logger.LogDebug($"Updating discount group {discountGroup.Name} ({discountGroup.Category}).");

                psDiscountGroup.name = discountGroup.Name;
                psDiscountGroup.category = discountGroup.Category;
                _psDiscountGroupRepository.SaveOrUpdate(psDiscountGroup);
            }
        }

        private void DeleteDiscountGroup(DiscountGroupEntity discountGroup, PsDiscountGroup psDiscountGroup)
        {
            if (psDiscountGroup != null && psDiscountGroup.deleted == 0)
            {
                _logger.LogDebug($"Deleting discount group {discountGroup.Name} ({discountGroup.Category}).");

                psDiscountGroup.deleted = 1;
                _psDiscountGroupRepository.SaveOrUpdate(psDiscountGroup);
            }
        }
    }
}
