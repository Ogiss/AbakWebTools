using AbakTools.Core.Domain.Address;
using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Framework;
using AbakTools.Core.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Core.Domain.Customer
{
    public class CustomerEntity : BusinessEntity
    {
        public virtual int? WebId { get; set; }
        public virtual string Code { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Nip { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual bool Active { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }
        public virtual string WebAccountLogin { get; protected set; }
        public virtual string WebAccountPassword { get; set; }
        public virtual ISet<AddressEntity> Addresses { get; set; } = new HashSet<AddressEntity>();
        public virtual ISet<CustomerDiscountEntity> Discounts { get; set; } = new HashSet<CustomerDiscountEntity>();

        public virtual bool IsArchived => IsDeleted || Synchronize == SynchronizeType.Deleted;

        protected CustomerEntity() { }

        public CustomerEntity(Guid guid)
        {
            Guard.NotEmpty(guid, nameof(guid));

            Guid = guid;
        }

        public virtual void SetCode(string code)
        {
            Guard.NotEmpty(code, nameof(code));

            Code = code;
        }

        public virtual void SetName(string name)
        {
            Guard.NotEmpty(name, nameof(name));

            Name = name;
        }

        public virtual void SetNip(string nip)
        {
            Nip = nip;
        }

        public virtual void SetEmail(string email)
        {
            Email = email;
        }

        public virtual void SetWebAccountLogin(string login)
        {
            WebAccountLogin = string.IsNullOrEmpty(login) ? null : login.Trim();
        }

        public virtual AddressEntity GetMainAddress()
        {
            var address = Addresses.Where(x => !x.IsArchived && x.IsDefaultInvoiceAddress).SingleOrDefault();

            if (address == null)
            {
                address = Addresses.Where(x => x.IsDefaultDeliveryAddress == true).SingleOrDefault();
            }

            if (address == null)
            {
                address = Addresses.FirstOrDefault();
                if (address != null)
                {
                    address.IsDefaultInvoiceAddress = true;
                }
            }

            if (address == null)
            {
                address = new AddressEntity();
                address.IsDefaultInvoiceAddress = true;
                address.Customer = this;
                Addresses.Add(address);
            }

            return address;
        }

        public virtual AddressEntity GetDefaultDeliveryAddress()
        {
            return Addresses.Where(x => x.IsDefaultDeliveryAddress == true).SingleOrDefault() ?? GetMainAddress();
        }

        public virtual void SetGroupDiscount(DiscountGroupEntity discountGroup, Discount discount, bool active = true)
        {
            var customerDiscount = Discounts.SingleOrDefault(x => x.DiscountGroup.Id == discountGroup.Id && x.Synchronize != SynchronizeType.Deleted);

            if (customerDiscount == null)
            {
                customerDiscount = new CustomerDiscountEntity(this, discountGroup, discount, active);
                Discounts.Add(customerDiscount);
            }
            else
            {
                customerDiscount.SetDiscount(discount);
                customerDiscount.ToggleActive(active);
            }
        }
    }
}
