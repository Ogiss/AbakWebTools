using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Enova;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    class EnovaDeletedDiscountGroupsImporter : EnovaImporterWithDateTimeStampBase<Guid>
    {
        private readonly IEnovaDictionaryItemRepository _enovaDictionaryItemRepository;
        private readonly IDiscountGroupRepository _discountGroupRepository;
        protected override ILogger Logger { get; }

        public EnovaDeletedDiscountGroupsImporter(
            ILogger<EnovaDeletedDiscountGroupsImporter> logger,
            IEnovaDictionaryItemRepository enovaDictionaryItemRepository,
            IDiscountGroupRepository discountGroupRepository) =>
            (Logger, _enovaDictionaryItemRepository, _discountGroupRepository) = (logger, enovaDictionaryItemRepository, discountGroupRepository);

        protected override IEnumerable<Guid> GetEntries(DateTime stampFrom, DateTime stampTo)
        {
            using var uow = UnitOfWorkProvider.CreateReadOnly();
            return _enovaDictionaryItemRepository.GetDeletedItemsGuidsAsync(stampFrom, stampTo).Result;
        }

        protected override void ProcessEntry(Guid guid)
        {
            using var uow = UnitOfWorkProvider.CreateReadOnly();
            var entity = _discountGroupRepository.Get(guid);

            if (entity != null)
            {
                Logger.LogDebug($"Delete discount group {entity.Name}");
                _discountGroupRepository.Delete(entity);
            }

            uow.Commit();
        }
    }
}
