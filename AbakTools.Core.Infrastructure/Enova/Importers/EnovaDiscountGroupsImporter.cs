﻿using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Enova;
using EnovaApi.Models.DiscountGroup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    class EnovaDiscountGroupsImporter : EnovaImporterWithLongStampBase<DiscountGroup>
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IEnovaDiscountGroupRepository _enovaDiscountGroupRepository;
        private readonly IDiscountGroupRepository _discountGroupRepository;

        protected override ILogger Logger => _logger;

        public EnovaDiscountGroupsImporter(
            ILogger<EnovaDiscountGroupsImporter> logger,
            IConfiguration configuration,
            IEnovaDiscountGroupRepository enovaDiscountGroupRepository,
            IDiscountGroupRepository discountGroupRepository)
            => (_logger, _configuration, _enovaDiscountGroupRepository, _discountGroupRepository) = (logger, configuration, enovaDiscountGroupRepository, discountGroupRepository);

        protected override IEnumerable<DiscountGroup> GetEntries(long stampFrom, long stampTo)
        {
            using var uow = UnitOfWorkProvider.CreateReadOnly();
            int defaultPriceDefId = int.Parse(_configuration.GetSection("EnovaSynchronization:DefaultPriceDefId").Value);
            return _enovaDiscountGroupRepository.GetModifiedDiscountGroupAsync(defaultPriceDefId, stampFrom, stampTo).Result;
        }

        protected override void ProcessEntry(DiscountGroup entry)
        {
            using var uow = UnitOfWorkProvider.Create();
            var entity = _discountGroupRepository.Get(entry.Guid);

            if(entity == null)
            {
                Logger.LogDebug($"Insert discount group {entry.Name}");
                entity = new DiscountGroupEntity(entry.Guid, entry.Category, entry.Name);
            }
            else
            {
                Logger.LogDebug($"Update discount group {entry.Name}");
                entity.SetName(entry.Name);
            }

            _discountGroupRepository.SaveOrUpdate(entity);
        }
    }
}
