using AbakTools.Core.Domain.Product;
using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Order
{
    public class OrderItemEntity : Entity
    {
        public virtual OrderEntity Order { get; set; }
        public virtual ProductEntity Product { get; set; }
        public virtual int? WebId { get; set; }
        public virtual int ItemNumber { get; set; }
        public virtual double Quantity { get; set; }
        public virtual double OrginalQuantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal Rebate { get; set; }
        public virtual bool RebateModified { get; set; }
        public virtual string TaxName { get; set; }
        public virtual decimal TaxValue { get; set; }
        public virtual string Description { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }
    }
}
