using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Enova;
using EnovaApi.Models.DiscountGroup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    class EnovaCustomerDiscountsImporter : EnovaImporterWithLongStampBase<CustomerDiscountGroup>
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IEnovaCustomerDiscountRepository _enovaCustomerDiscountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiscountGroupRepository _discountGroupRepository;

        protected override ILogger Logger => _logger;

        public EnovaCustomerDiscountsImporter(
            ILogger<EnovaCustomerDiscountsImporter> logger,
            IConfiguration configuration,
            IEnovaCustomerDiscountRepository enovaCustomerDiscountRepository,
            ICustomerRepository customerRepository,
            IDiscountGroupRepository discountGroupRepository)
            => (_logger, _configuration, _enovaCustomerDiscountRepository, _customerRepository, _discountGroupRepository)
            = (logger, configuration, enovaCustomerDiscountRepository, customerRepository, discountGroupRepository);

        protected override IEnumerable<CustomerDiscountGroup> GetEntries(long stampFrom, long stampTo)
        {
            using var uow = UnitOfWorkProvider.CreateReadOnly();
            int defaultPriceDefId = int.Parse(_configuration.GetSection("EnovaSynchronization:DefaultPriceDefId").Value);
            return _enovaCustomerDiscountRepository.GetModifiedCustomerDiscountsAsync(defaultPriceDefId, stampFrom, stampTo).Result;
        }

        protected override void ProcessEntry(CustomerDiscountGroup entry)
        {
            using var uow = UnitOfWorkProvider.Create();
            var customer = _customerRepository.Get(entry.CustomerGuid);

            if (customer != null)
            {
                var discountGroup = _discountGroupRepository.Get(entry.DiscountGroupGuid);

                if (discountGroup != null)
                {
                    _logger.LogDebug($"Import customer discount for {customer.Code}, group: {discountGroup.Name}, diccount: {entry.Discount:P}");
                    customer.SetGroupDiscount(discountGroup, Discount.Of(entry.Discount));
                    _customerRepository.SaveOrUpdate(customer);
                }
            }

            uow.Commit();
        }
    }
}
