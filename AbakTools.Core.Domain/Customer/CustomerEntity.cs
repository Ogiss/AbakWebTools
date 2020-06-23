using AbakTools.Core.Domain.Address;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbakTools.Core.Domain.Customer
{
    public class CustomerEntity : BusinessEntity
    {
        public virtual int? WebId { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Nip { get; set; }
        public virtual string Email { get; set; }
        public virtual bool Active { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }
        public virtual string WebAccountLogin { get; set; }
        public virtual string WebAccountPassword { get; set; }

        public virtual ISet<AddressEntity> Addresses { get; set; } = new HashSet<AddressEntity>();


        public virtual bool IsDeleting => Synchronize == SynchronizeType.Deleted;

        public virtual AddressEntity GetMainAddress()
        {
            var address = Addresses.Where(x => x.IsDefaultInvoiceAddress).SingleOrDefault();

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

            return address;
        }

        public virtual AddressEntity GetDefaultDeliveryAddress()
        {
            return Addresses.Where(x => x.IsDefaultDeliveryAddress == true).SingleOrDefault() ?? GetMainAddress();
        }
    }
}
