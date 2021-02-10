using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Enova;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Importers
{
    class EnovaCustomersImporter : EnovaImporterBase<Guid>
    {
        private readonly ILogger _logger;
        private readonly IEnovaCustomerRepository _enovaCustomerRepository;
        private readonly ICustomerRepository _customerRepository;

        public EnovaCustomersImporter(
            ILogger<EnovaCustomersImporter> logger,
            IEnovaCustomerRepository enovaCustomerRepository,
            ICustomerRepository customerRepository)
            => (_logger, _enovaCustomerRepository, _customerRepository) = (logger, enovaCustomerRepository, customerRepository);

        protected async override Task<IEnumerable<Guid>> GetEntriesAsync(long stampFrom, long stampTo)
        {
            return await _enovaCustomerRepository.GetModifiedCustomersGuidsAsync(stampFrom, stampTo);
        }

        protected override void ProcessEntry(Guid guid)
        {
            var entry = _enovaCustomerRepository.Get(guid);

            if(entry != null && entry.Stamp <= StampTo)
            {
                _logger.LogDebug($"Importing customer {entry.Code} - {entry.Name}");

                var entity = _customerRepository.Get(guid);

                if(entity == null)
                {
                    entity = new CustomerEntity(guid);
                }

                UpdateCustomer(entity, entry);

                _customerRepository.SaveOrUpdate(entity);
            }
        }

        private void UpdateCustomer(CustomerEntity entity, EnovaApi.Models.Customer.Customer entry)
        {
            entity.SetCode(entry.Code);
            entity.SetName(entry.Name);
            entity.SetNip(entry.Nip);
            entity.SetEmail(entry.Email);

            UpdateMainAddress(entity, entry);
            UpdateWebAccount(entity, entry);
        }

        private void UpdateMainAddress(CustomerEntity entity, EnovaApi.Models.Customer.Customer entry)
        {
            var mainAddress = entity.GetMainAddress();

            mainAddress.AddressLine1 = entry.MainAddress.AddressLine1;
            mainAddress.PostalCode = entry.MainAddress.PostalCode;
            mainAddress.City = entry.MainAddress.City;
        }

        private void UpdateWebAccount(CustomerEntity entity, EnovaApi.Models.Customer.Customer entry)
        {
            entity.SetWebAccountLogin(entry.WebAccount.Login);
        }
    }
}
