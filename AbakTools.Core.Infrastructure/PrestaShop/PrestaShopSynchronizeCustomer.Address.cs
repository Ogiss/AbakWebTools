using AbakTools.Core.Domain.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using PsCustomer = Bukimedia.PrestaSharp.Entities.customer;
using PsAddress = Bukimedia.PrestaSharp.Entities.address;
using AbakTools.Core.Domain.Address;
using Microsoft.Extensions.Logging;
using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    internal partial class PrestaShopSynchronizeCustomer
    {
        private void SynchronizeAddresses(CustomerEntity customer, PsCustomer psCustomer)
        {
            var address = customer.GetMainAddress();

            if (address != null && address.IsValid)
            {
                var psAddress = GetPsAddress(address.WebId);

                if(psAddress == null && address.WebId.HasValue)
                {
                    address.WebId = null;
                }

                if(psAddress == null)
                {
                    if (!address.IsArchived)
                    {
                        psAddress = InsertAddress(psCustomer, address, "Domyślny");
                    }
                }
                else
                {
                    if (address.IsArchived)
                    {
                        DeleteAddress(address, psAddress);
                    }
                    else
                    {
                        UpdateAddress(address, psAddress);
                    }
                }

                if(psAddress != null)
                {
                    psAddress = SaveOrUpdateAddress(address, psAddress);
                }

                if (!address.WebId.HasValue)
                {
                    address.WebId = (int?)psAddress?.id;
                }

                address.Synchronize = SynchronizeType.Synchronized;
            }
        }

        private PsAddress InsertAddress(PsCustomer psCustomer, AddressEntity address, string alias)
        {
            var psAddress = new PsAddress();
            psAddress.id_customer = psCustomer.id;
            psAddress.alias = alias;

            var country = prestaShopClient.DefaultCountry;
            psAddress.id_country = country.id;

            UpdateAddress(address, psAddress);

            return psAddress;
        }

        private void UpdateAddress(Domain.Address.AddressEntity address, PsAddress psAddress)
        {
            psAddress.company = address.Customer.Name;
            psAddress.lastname = " ";
            psAddress.firstname = " ";
            psAddress.vat_number = address.Customer.Nip;
            psAddress.address1 = address.AddressLine1;
            psAddress.city = address.City;
            psAddress.postcode = address.PostalCode;
        }

        private void DeleteAddress(Domain.Address.AddressEntity address, PsAddress psAddress)
        {
            if (psAddress != null)
            {
                psAddress.deleted = 1;
            }

            address.IsDeleted = true;
        }

        private PsAddress GetPsAddress(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    return prestaShopClient.AddressFactory.Get(id.Value);
                }
                catch (Bukimedia.PrestaSharp.PrestaSharpException ex)
                {
                    if (ex.ResponseHttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }

                    throw;
                }
            }

            return null;
        }

        private PsAddress SaveOrUpdateAddress(AddressEntity address, PsAddress psAddress)
        {
            if (psAddress.id.HasValue && psAddress.id.Value > 0)
            {
                logger.LogInformation($"Update customer id: {address.Id}, name: {address.Name}");
                prestaShopClient.AddressFactory.Update(psAddress);
            }
            else
            {
                logger.LogInformation($"Add new customer id: {address.Id}, name: {address.Name}");
                psAddress = prestaShopClient.AddressFactory.Add(psAddress);
            }

            return psAddress;
        }
    }
}
