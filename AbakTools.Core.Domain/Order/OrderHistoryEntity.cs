using System;

namespace AbakTools.Core.Domain.Order
{
    public class OrderHistoryEntity : Entity
    {
        public virtual OrderEntity Order { get; set; }
        public virtual OrderStatusEntity Status { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
