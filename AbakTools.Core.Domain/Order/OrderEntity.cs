using AbakTools.Core.Domain.Address;
using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;

namespace AbakTools.Core.Domain.Order
{
    public class OrderEntity : BusinessEntity
    {
        public virtual CustomerEntity Customer { get; set; }
        public virtual AddressEntity DeliveryAddress { get; set; }
        public virtual AddressEntity InvoiceAddress { get; set; }
        public virtual int? WebId { get; set; }
        public virtual decimal TotalProductsValueNet { get; set; }
        public virtual decimal TotalProductsValueGross { get; set; }
        public virtual decimal TotalValueNet { get; set; }
        public virtual decimal TotalValueGross { get; set; }
        public virtual string Packer { get; set; }
        public virtual TransportType Transport { get; set; }
        public virtual DateTime? DeliveryDate { get; set; }
        public virtual string DeliveryTimeOfDay { get; set; }
        public virtual int Term { get; set; }
        public virtual Guid? InvoiceGuid { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual OrderStatusEntity Status { get; set; }
        public virtual bool? UrgentOrder { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }

        public virtual ISet<OrderItemEntity> Items { get; set; } = new HashSet<OrderItemEntity>();
        public virtual ISet<OrderHistoryEntity> History { get; set; } = new HashSet<OrderHistoryEntity>();
    }
}
