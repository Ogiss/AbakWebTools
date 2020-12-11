using AbakTools.Core.Domain.Address;
using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Domain.Policies;
using AbakTools.Core.Domain.Product;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Core.Domain.Order
{
    public class OrderEntity : BusinessEntity
    {
        public virtual CustomerEntity Customer { get; protected set; }
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
        public virtual int? Term { get; set; }
        public virtual Guid? InvoiceGuid { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual OrderStateEntity State { get; protected set; }
        public virtual bool? UrgentOrder { get; set; }
        public virtual OrderSourceType? OrderSource { get; protected set; }
        public virtual SynchronizeType Synchronize { get; set; }

        public virtual ISet<OrderRowEntity> Rows { get; set; } = new HashSet<OrderRowEntity>();
        public virtual ISet<OrderHistoryEntity> History { get; set; } = new HashSet<OrderHistoryEntity>();

        protected OrderEntity() { }

        public static OrderEntity Create(CustomerEntity customer, OrderSourceType orderSource)
        {
            Guard.NotNull(customer, nameof(customer));

            return new OrderEntity
            {
                Customer = customer,
                OrderSource = orderSource
            };
        }

        public virtual void ChangeState(OrderStateEntity state)
        {
            if (state != null)
            {
                var lastState = GetLastState();

                if (State == null || State.Id != state.Id)
                {
                    State = state;
                }

                if (lastState == null || lastState.Id != state.Id)
                {
                    var history = OrderHistoryEntity.Create(this, state);
                    History.Add(history);
                }
            }
        }

        public virtual IList<OrderRowEntity> GetRows()
        {
            return Rows.Where(x => x.Synchronize != SynchronizeType.Deleted).ToList();
        }

        public virtual OrderHistoryEntity GetLastHistory()
        {
            return History.AsQueryable().OrderBy(x => x.Id).LastOrDefault();
        }

        public virtual OrderStateEntity GetLastState()
        {
            return GetLastHistory()?.State;
        }

        public virtual OrderRowEntity GetRowByWebId(int webId)
        {
            return Rows.AsQueryable().SingleOrDefault(x => x.WebId == webId);
        }

        public virtual void AddRow(OrderRowEntity row)
        {
            Rows.Add(row);
            row.ItemNumber = Rows.Count;
            RecalcTotals();
        }

        public virtual OrderRowEntity AddRow(ProductEntity product, double quantity, IProductPricePolicy pricePolicy)
        {
            var row = OrderRowEntity.Create(this, product, quantity, pricePolicy);
            AddRow(row);

            return row;
        }

        public virtual void RemoveRow(OrderRowEntity row)
        {
            if (Rows.Contains(row))
            {
                row.Synchronize = SynchronizeType.Deleted;
                RenumerableRows();
                RecalcTotals();
            }
        }

        private void RecalcTotals()
        {
            TotalValueNet = GetRows().Sum(x => x.TotalValueNet);
            TotalValueGross = GetRows().Sum(x => x.TotalValueGross);
            TotalProductsValueNet = TotalValueNet;
            TotalProductsValueGross = TotalValueGross;
        }

        private void RenumerableRows()
        {
            int number = 1;

            foreach (var row in GetRows().OrderBy(x => x.ItemNumber))
            {
                row.ItemNumber = number++;
            }
        }
    }
}
