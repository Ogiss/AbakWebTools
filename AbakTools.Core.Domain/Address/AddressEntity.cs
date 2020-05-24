using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Address
{
    public class AddressEntity : BusinessEntity
    {
        public virtual CustomerEntity Customer {get;set;}
        public virtual int? WebId { get; set; }
        public virtual bool Active { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string City { get; set; }
        public virtual string Name { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }
        public virtual bool IsDefaultInvoiceAddress { get; set; }
        public virtual bool IsDefaultDeliveryAddress { get; set; }
    }
}
