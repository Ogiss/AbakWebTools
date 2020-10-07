using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PsCustomer = Bukimedia.PrestaSharp.Entities.customer;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    internal partial class PrestaShopSynchronizeCustomer
    {
        private void ImportCustomers()
        {
            logger.LogDebug("Starting import custopers from PrestaShop");

            try
            {
                SynchronizeStampEntity synchronizeStamp = null;
                DateTime stampTo = DateTime.Now;

                using (var uow = unitOfWorkProvider.CreateReadOnly())
                {
                    synchronizeStamp = synchronizeStampRepository.Get(SynchronizeCodes.Customer, SynchronizeDirectionType.Import);
                }

                DateTime stampFrom = synchronizeStamp?.DateTimeStamp ?? DateTime.MinValue;

                IReadOnlyCollection<long> psCustomerIds = psCustomerRepository.GetAllModifiedBetween(stampFrom, stampTo);

                if (Enumerable.Any(psCustomerIds))
                {
                    logger.LogDebug($"Starting synchronize {psCustomerIds.Count} orders");

                    var parallelOptions = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 1
                    };

                    Parallel.ForEach(psCustomerIds, parallelOptions, x => ProcessPsCustomerId(x, stampTo));

                    if (synchronizeStamp == null)
                    {
                        synchronizeStamp = SynchronizeStampFactory.Create(SynchronizeCodes.Order, SynchronizeDirectionType.Import);
                    }

                    synchronizeStamp.DateTimeStamp = stampTo;

                    using (var uow = unitOfWorkProvider.Create())
                    {
                        synchronizeStampRepository.SaveOrUpdate(synchronizeStamp);
                        uow.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Synchronize orders from PrestaShop error: {ex}");
            }

        }

        private void ProcessPsCustomerId(long id, DateTime stampTo)
        {
            var psCustomer = psCustomerRepository.Get(id);

            if (psCustomer != null && DateTimeHelper.ParseInvariant(psCustomer.date_upd) <= stampTo)
            {
                using (var uow = unitOfWorkProvider.Create())
                {
                    var order = customerRepository.GetByWebId((int)psCustomer.id);

                    if (order == null)
                    {
                        order = InsertPsCustomer(psCustomer);
                    }
                    else
                    {
                        UpdatePsCustomer(order, psCustomer);
                    }

                    order.WebId = (int)psCustomer.id;

                    customerRepository.SaveOrUpdate(order);
                    uow.Commit();
                }
            }

        }

        private void UpdatePsCustomer(CustomerEntity order, PsCustomer psCustomer)
        {
            throw new NotImplementedException();
        }

        private CustomerEntity InsertPsCustomer(PsCustomer psCustomer)
        {
            throw new NotImplementedException();
        }
    }
}
