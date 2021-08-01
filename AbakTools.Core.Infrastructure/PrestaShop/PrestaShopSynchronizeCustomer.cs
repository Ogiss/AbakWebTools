using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework.Domain;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using Bukimedia.PrestaSharp.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PsCustomer = Bukimedia.PrestaSharp.Entities.customer;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    internal partial class PrestaShopSynchronizeCustomer : IPrestaShopSynchronizeCustomer
    {
        private const string DefaultCustomerPassword = "cgY563_Cf#90a";

        private readonly ILogger logger;
        private readonly IUnitOfWorkProvider unitOfWorkProvider;
        private readonly ISynchronizeStampRepository synchronizeStampRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IPrestaShopClient prestaShopClient;
        private readonly IPSCustomerRepository psCustomerRepository;

        public PrestaShopSynchronizeCustomer(
            ILogger<PrestaShopSynchronizeCustomer> _logger,
            IUnitOfWorkProvider _unitOfWorkProvider,
            ISynchronizeStampRepository _synchronizeStampRepository,
            ICustomerRepository _customerRepository,
            IPrestaShopClient _prestaShopClient,
            IPSCustomerRepository _psCustomerRepository)
        {
            logger = _logger;
            unitOfWorkProvider = _unitOfWorkProvider;
            synchronizeStampRepository = _synchronizeStampRepository;
            customerRepository = _customerRepository;
            prestaShopClient = _prestaShopClient;
            psCustomerRepository = _psCustomerRepository;
        }

        public Task DoWork(CancellationToken stoppingToken)
        {
            //ImportCustomers();
            ExportCustomers();
            return Task.CompletedTask;
        }

        private void ExportCustomers()
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

                    if (synchronizeStamp == null)
                    {
                        synchronizeStamp = SynchronizeStampFactory.Create(SynchronizeCodes.Customer, SynchronizeDirectionType.Export);
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
                logger.LogError($"Synchronize customers to PrestaShop error: {ex.ToString()}");
            }
        }

        private void ProcessCustomerId(int id, DateTime stampTo)
        {
            try
            {
                using (var uow = unitOfWorkProvider.Create())
                {
                    var customer = customerRepository.Get(id);

                    if (customer.ModificationDate <= stampTo)
                    {
                        var psCustomer = GetPsCustomer(customer.WebId);

                        if (psCustomer == null && customer.WebId.HasValue)
                        {
                            customer.WebId = null;
                        }

                        if (psCustomer == null)
                        {
                            psCustomer = psCustomerRepository.GetByEmail(customer.WebAccountLogin);
                        }

                        if (psCustomer == null)
                        {
                            if (!customer.IsArchived)
                            {
                                psCustomer = InsertCustomer(customer);
                            }
                        }
                        else
                        {
                            if (customer.IsArchived)
                            {
                                DeleteCustomer(customer, psCustomer);
                            }
                            else
                            {
                                UpdateCustomer(customer, psCustomer);
                            }
                        }

                        if (psCustomer != null)
                        {
                            if (psCustomer.associations.groups.Count == 0)
                            {
                                psCustomer.associations.groups.AddRange(prestaShopClient.AllGroupsIds.Select(x =>
                                    new Bukimedia.PrestaSharp.Entities.AuxEntities.group { id = x }));
                            }

                            if (psCustomer.id_default_group == 0)
                            {
                                psCustomer.id_default_group = prestaShopClient.DefaultGroupId;
                            }

                            psCustomer = SaveOrUpdateCustomer(customer, psCustomer);
                            SynchronizeAddresses(customer, psCustomer);
                        }

                        if (!customer.WebId.HasValue)
                        {
                            customer.WebId = (int?)psCustomer?.id;
                        }

                        customer.Synchronize = SynchronizeType.Synchronized;
                        customerRepository.SaveOrUpdate(customer);
                        uow.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Sunchronize customerr error Id: {id}.{Environment.NewLine}{ex.ToString()}");
            }
        }

        private PsCustomer InsertCustomer(CustomerEntity customer)
        {
            var psCustomer = new customer();

            psCustomer.passwd = DefaultCustomerPassword;

            UpdateCustomer(customer, psCustomer);

            return psCustomer;
        }

        private PsCustomer GetPsCustomer(int? id)
        {
            return id.HasValue ? psCustomerRepository.Get(id.Value) : null;
        }

        private void DeleteCustomer(CustomerEntity customer, customer psCustomer)
        {
            if (psCustomer != null)
            {
                psCustomer.active = 0;
            }

            customer.IsDeleted = true;
        }

        private void UpdateCustomer(CustomerEntity customer, customer psCustomer)
        {
            psCustomer.company = customer.Name;
            psCustomer.email = customer.WebAccountLogin.Trim();
            psCustomer.active = 1;
            //psCustomer.lastname = Functions.GetPrestaShopLastname(customer.Code);
            psCustomer.lastname = " ";
            psCustomer.firstname = " ";

        }

        private PsCustomer SaveOrUpdateCustomer(CustomerEntity customer, PsCustomer psCustomer)
        {
            if (psCustomer.id.HasValue && psCustomer.id.Value > 0)
            {
                logger.LogInformation($"Update customer id: {customer.Id}, name: {customer.Code}-{customer.Name}");
                prestaShopClient.CustomerFactory.Update(psCustomer);
            }
            else
            {
                logger.LogInformation($"Add new customer id: {customer.Id}, name: {customer.Code}-{customer.Name}");
                psCustomer = prestaShopClient.CustomerFactory.Add(psCustomer);
            }

            return psCustomer;
        }
    }
}
