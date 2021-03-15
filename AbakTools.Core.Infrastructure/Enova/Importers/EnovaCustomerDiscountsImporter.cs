using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Enova;
using EnovaApi.Models.DiscountGroup;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    class EnovaCustomerDiscountsImporter : EnovaImporterWithLongStampBase<CustomerDiscountGroup>
    {
        private readonly IConfiguration _configuration;
        private readonly IEnovaCustomerDiscountRepository _enovaCustomerDiscountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiscountGroupRepository _discountGroupRepository;

        public EnovaCustomerDiscountsImporter(
            IConfiguration configuration,
            IEnovaCustomerDiscountRepository enovaCustomerDiscountRepository,
            ICustomerRepository customerRepository,
            IDiscountGroupRepository discountGroupRepository)
            => (_configuration, _enovaCustomerDiscountRepository, _customerRepository, _discountGroupRepository)
            = (configuration, enovaCustomerDiscountRepository, customerRepository, discountGroupRepository);

        protected override async Task<IEnumerable<CustomerDiscountGroup>> GetEntriesAsync(long stampFrom, long stampTo)
        {
            int defaultPriceDefId = int.Parse(_configuration.GetSection("EnovaSynchronization:DefaultPriceDefId").Value);
            return await _enovaCustomerDiscountRepository.GetModifiedCustomerDiscountsAsync(defaultPriceDefId, stampFrom, stampTo);
        }

        protected override void ProcessEntry(CustomerDiscountGroup entry)
        {
            var customer = _customerRepository.Get(entry.CustomerGuid);

            if (customer != null)
            {
                var discountGroup = _discountGroupRepository.Get(entry.DiscountGroupGuid);

                if (discountGroup != null)
                {
                    customer.SetGroupDiscount(discountGroup, Discount.Of(entry.Discount));
                    _customerRepository.SaveOrUpdate(customer);
                }
            }
        }
    }
}
