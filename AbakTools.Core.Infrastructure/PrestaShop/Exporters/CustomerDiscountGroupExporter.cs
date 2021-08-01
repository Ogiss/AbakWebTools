using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Services;
using AbakTools.Core.Framework.UnitOfWork;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AbakTools.Core.Domain.Customer.Specifications;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using System;
using PSCustomerDiscountGroup = Bukimedia.PrestaSharp.Entities.customer_discount_group;
using AbakTools.Core.Domain.Common.Projections;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    class CustomerDiscountGroupExporter : PrestaShopExporterBase<int>
    {
        private readonly ICustomerDiscountGroupRepository _customerDiscountGroupRepository;
        private readonly IPsCustomerDiscountGroupRepository _psCustomerDiscountGroupRepository;

        public CustomerDiscountGroupExporter(
            ILogger<CustomerDiscountGroupExporter> logger,
            IUnitOfWorkProvider unitOfWorkProvider,
            ISynchronizeStampService synchronizeStampService,
            ICustomerDiscountGroupRepository customerDiscountGroupRepository,
            IPsCustomerDiscountGroupRepository psCustomerDiscountGroupRepository)
            : base(logger, unitOfWorkProvider, synchronizeStampService)
        {
            _customerDiscountGroupRepository = customerDiscountGroupRepository;
            _psCustomerDiscountGroupRepository = psCustomerDiscountGroupRepository;
        }

        protected override IEnumerable<int> GetExportingEntries(CancellationToken cancelerationToken)
        {
            var specification = CustomerDiscountToExportSpecyfication.Of(StampFrom, StampTo);

            using (var uow = UnitOfWorkProvider.CreateReadOnly())
            {
                return _customerDiscountGroupRepository.GetList(specification, EntityIdProjection<CustomerDiscountEntity>.Create());
            }
        }

        protected override void ProcessEntry(int id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var entity = _customerDiscountGroupRepository.Get(id);

                if (entity.Stamp <= StampTo)
                {
                    var psEntity = _psCustomerDiscountGroupRepository.Get(entity.Customer.WebId.Value, entity.DiscountGroup.WebId.Value);

                    if (!entity.DiscountActive)
                    {
                        Deactivate(entity, psEntity);
                    }
                    else
                    {
                        if (psEntity == null)
                        {
                            psEntity = Insert(entity);
                        }
                        else
                        {
                            Update(entity, psEntity);
                        }
                    }
                }

                entity.MakeSynchronized();
                _customerDiscountGroupRepository.SaveOrUpdate(entity);
                uow.Commit();
            }
        }

        private PSCustomerDiscountGroup Insert(CustomerDiscountEntity entity)
        {
            Logger.LogDebug($"Inserting customer discount for customer: {entity.Customer.Code} ({entity.Customer.Id}), " +
                $"group: {entity.DiscountGroup.Name}({entity.DiscountGroup.Id}), discount: {entity.Discount}");

            var psEntity = new PSCustomerDiscountGroup
            {
                id_customer = (long)entity.Customer.WebId,
                id_discount_group = (long)entity.DiscountGroup.WebId,
                discount = entity.Discount
            };

            psEntity = _psCustomerDiscountGroupRepository.SaveOrUpdate(psEntity);

            return psEntity;
        }

        private void Update(CustomerDiscountEntity entity, PSCustomerDiscountGroup psEntity)
        {
            if (!Equals((decimal)entity.Discount, psEntity.discount))
            {
                Logger.LogDebug($"Updating customer discount for customer: {entity.Customer.Code} ({entity.Customer.Id}), " +
                    $"group: {entity.DiscountGroup.Name}({entity.DiscountGroup.Id}), discount: {entity.Discount}");

                psEntity.discount = entity.Discount;
                _psCustomerDiscountGroupRepository.SaveOrUpdate(psEntity);
            }
        }

        private void Deactivate(CustomerDiscountEntity entity, PSCustomerDiscountGroup psEntity)
        {
            if (psEntity != null)
            {
                Logger.LogDebug($"Deleting customer discount for customer: {entity.Customer.Code} ({entity.Customer.Id}), " +
                       $"group: {entity.DiscountGroup.Name}({entity.DiscountGroup.Id}), discount: {entity.Discount}");

                _psCustomerDiscountGroupRepository.Delete(psEntity);
            }
        }
    }
}
