using AbakTools.Core.Domain.Policies;
using AbakTools.Core.Domain.Product;
using AbakTools.Core.Framework;
using AbakTools.Core.Framework.Domain;
using System;

namespace AbakTools.Core.Domain.Order
{
    public class OrderRowEntity : Entity
    {
        public virtual OrderEntity Order { get; protected set; }
        public virtual ProductEntity Product { get; protected set; }
        public virtual int? WebId { get; set; }
        public virtual int ItemNumber { get; set; }
        public virtual double Quantity { get; protected set; }
        public virtual double OrginalQuantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal Rebate { get; set; }
        public virtual bool RebateModified { get; set; }
        public virtual string TaxName { get; set; }
        public virtual decimal? TaxValue { get; set; }
        public virtual string Description { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }

        public virtual decimal PriceWithRebate => GetPriceWithRebate();
        public virtual decimal TotalValueNet => GetTotalValueNet();
        public virtual decimal TotalValueGross => GetTotalValueGross();

        protected OrderRowEntity() { }

        public static OrderRowEntity Create(OrderEntity order, ProductEntity product, double quantity, IProductPricePolicy pricePolicy)
        {
            Guard.NotNull(order, nameof(order));
            Guard.NotNull(order.Customer, nameof(order.Customer));
            Guard.NotNull(product, nameof(product));

            var priceInfo = pricePolicy.GetProductPriceInfoForCustomer(product, order.Customer);

            return new OrderRowEntity
            {
                Order = order,
                Product = product,
                Quantity = quantity,
                OrginalQuantity = quantity,
                Price = priceInfo.Price,
                Rebate = priceInfo.Rebate,
                TaxName = product.Tax?.Name,
                TaxValue = (product.Tax?.Rate ?? 0) / 100M
            };
        }

        public virtual void ChangeQuantity(double quantity)
        {
            if (quantity != Quantity)
            {
                Quantity = quantity;
            }
        }

        private decimal GetPriceWithRebate()
        {
            return Price * (1 - Rebate);
        }

        private decimal GetTotalValueNet()
        {
            return decimal.Round(GetPriceWithRebate() * (decimal)Quantity, 2);
        }

        private decimal GetTotalValueGross()
        {
            return decimal.Round(GetTotalValueNet() * (1 + (TaxValue ?? 0)), 2);
        }
    }
}
