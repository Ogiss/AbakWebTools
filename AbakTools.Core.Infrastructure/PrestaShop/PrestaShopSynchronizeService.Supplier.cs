using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Synchronize;
using System;
using System.Collections.Generic;
using System.Linq;
using Bukimedia.PrestaSharp.Entities;
using Microsoft.Extensions.Logging;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public partial class PrestaShopSynchronizeService
    {
        private void SynchronizeSuppliers()
        {
            SynchronizeStampEntity synchronizeStamp = null;
            long stampFrom = 0;
            long stampTo = 0;

            using (var uow = unitOfWorkProvider.CreateReadOnly())
            {
                synchronizeStamp = synchronizeStampRepository.Get(SynchronizeCodes.Supplier, SynchronizeDirectionType.Export);
                stampTo = supplierRepository.GetDbtsAsync().Result;
            }

            stampFrom = synchronizeStamp?.Stamp ?? 0;

            if (stampFrom < stampTo)
            {
                DateTime startDt = DateTime.Now;

                logger.LogDebug("Starting synchronize suppliers");

                IReadOnlyCollection<SupplierEntity> suppliers = null;

                using (var uow = unitOfWorkProvider.CreateReadOnly())
                {
                    suppliers = supplierRepository.GetAllModified(stampFrom, stampTo);
                }

                if (suppliers.Any())
                {
                    foreach (var supplier in suppliers)
                    {
                        ProcessSupplier(supplier);
                    }
                }

                if(synchronizeStamp == null)
                {
                    synchronizeStamp = SynchronizeStampFactory.Create(SynchronizeCodes.Supplier, SynchronizeDirectionType.Export);
                }

                synchronizeStamp.Stamp = stampTo;

                using(var uow = unitOfWorkProvider.Create())
                {
                    synchronizeStampRepository.SaveOrUpdate(synchronizeStamp);
                    uow.Commit();
                }

                logger.LogDebug($"Synchronize suppliers finished at {(DateTime.Now - startDt).TotalSeconds} sec.");
            }
        }

        private void ProcessSupplier(SupplierEntity supplier)
        {
            try
            {
                Dictionary<string, string> dtn = new Dictionary<string, string>();
                dtn.Add("name", supplier.Name);

                var psSupplier = prestaShopClient.SupplierFactory.GetByFilter(dtn, null, null).SingleOrDefault();

                if (psSupplier == null)
                {
                    psSupplier = new supplier();
                    psSupplier.active = 1;
                    psSupplier.name = supplier.Name;
                    prestaShopClient.SupplierFactory.Add(psSupplier);
                }
            }
            catch(Exception ex)
            {
                logger.LogError($"Synchronize suplier {supplier.Name} error.{Environment.NewLine}{ex}");
            }
        }
    }
}
