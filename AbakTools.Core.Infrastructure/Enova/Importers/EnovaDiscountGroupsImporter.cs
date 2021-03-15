using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Enova;
using EnovaApi.Models.DiscountGroup;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    class EnovaDiscountGroupsImporter : EnovaImporterWithLongStampBase<DiscountGroup>
    {
        private readonly IConfiguration _configuration;
        private readonly IEnovaDiscountGroupRepository _enovaDiscountGroupRepository;
        private readonly IDiscountGroupRepository _discountGroupRepository;

        public EnovaDiscountGroupsImporter(
            IConfiguration configuration,
            IEnovaDiscountGroupRepository enovaDiscountGroupRepository,
            IDiscountGroupRepository discountGroupRepository)
            => (_configuration, _enovaDiscountGroupRepository, _discountGroupRepository) = (configuration, enovaDiscountGroupRepository, discountGroupRepository);

        protected override async Task<IEnumerable<DiscountGroup>> GetEntriesAsync(long stampFrom, long stampTo)
        {
            int defaultPriceDefId = int.Parse(_configuration.GetSection("EnovaSynchronization:DefaultPriceDefId").Value);
            return await _enovaDiscountGroupRepository.GetModifiedDiscountGroupAsync(defaultPriceDefId, stampFrom, stampTo);
        }

        protected override void ProcessEntry(DiscountGroup entry)
        {
            var entity = _discountGroupRepository.Get(entry.Guid);

            if(entity == null)
            {
                entity = new DiscountGroupEntity(entry.Guid, entry.Category, entry.Name);
            }
            else
            {
                entity.SetName(entry.Name);
            }

            _discountGroupRepository.SaveOrUpdate(entity);
        }
    }
}
