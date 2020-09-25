using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Order
{
    public class OrderStatusEntity : BusinessEntity
    {
        public virtual int? WebId { get; set; }
        public virtual string Name { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }
    }
}
