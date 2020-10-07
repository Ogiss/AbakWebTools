using AbakTools.Core.Framework;
using System;

namespace AbakTools.Core.Domain.Order
{
    public class OrderHistoryEntity : Entity
    {
        public virtual OrderEntity Order { get; protected set; }
        public virtual OrderStateEntity State { get; protected set; }
        public virtual DateTime Date { get; protected set; }

        protected OrderHistoryEntity() { }

        public static OrderHistoryEntity Create(OrderEntity order, OrderStateEntity state)
        {
            return Create(order, state, null);
        }

        public static OrderHistoryEntity Create(OrderEntity order, OrderStateEntity state, DateTime? date)
        {
            Guard.NotNull(order, nameof(order));
            Guard.NotNull(state, nameof(state));

            date = date ?? DateTime.Now;

            return new OrderHistoryEntity
            {
                Order = order,
                State = state,
                Date = date.Value
            };
        }
    }
}
