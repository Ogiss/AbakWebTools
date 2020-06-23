using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    class PrestaShopSynchronizeCustomer : IPrestaShopSynchronizeCustomer
    {
        private readonly ILogger logger;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ISynchronizeStampRepository synchronizeStampRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IPrestaShopClient prestaShopClient;

        public PrestaShopSynchronizeCustomer(
            ILogger<PrestaShopSynchronizeCustomer> _logger,
            IUnitOfWorkProvider _unitOfWorkProvider,
            ISynchronizeStampRepository _synchronizeStampRepository,
            ICustomerRepository _customerRepository,
            IPrestaShopClient _prestaShopClient)
        {
            logger = _logger;
            unitOfWorkProvider = _unitOfWorkProvider;
            synchronizeStampRepository = _synchronizeStampRepository;
            customerRepository = _customerRepository;
            prestaShopClient = _prestaShopClient;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Synchronize();

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private void Synchronize()
        {
            try
            {
                SynchronizeStampEntity synchronizeStamp = null;
                DateTime stampTo = DateTime.Now;

                using (var uow = unitOfWorkProvider.CreateReadOnly())
                {
                    synchronizeStamp = synchronizeStampRepository.Get(SynchronizeCodes.Customer, SynchronizeDirectionType.Export);
                }

                DateTime stampFrom = synchronizeStamp?.DateTimeStamp ?? DateTime.MinValue;

                IReadOnlyCollection<int> customerIds = null;

                using (var uow = unitOfWorkProvider.CreateReadOnly())
                {
                    customerIds = customerRepository.GetModifiedCustomersIdsWithWebAccount(stampFrom, stampTo);
                }

                if (Enumerable.Any(customerIds))
                {
                    logger.LogDebug($"Starting synchronize {customerIds.Count} customers");

                    var parallelOptions = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 1
                    };

                    Parallel.ForEach(customerIds, parallelOptions, x => ProcessCustomerId(x, stampTo));
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Synchronize customers to PrestaShop error: {ex.ToString()}");
            }
        }

        private void ProcessCustomerId(int id, DateTime stampTo)
        {
            using (var uow = unitOfWorkProvider.Create())
            {
                var customer = customerRepository.Get(id);

                if (customer.ModificationDate <= stampTo)
                {

                }
            }
        }
    }
}
