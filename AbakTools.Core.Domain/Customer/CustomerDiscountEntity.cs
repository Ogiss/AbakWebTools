using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Customer
{
    public class CustomerDiscountEntity : GuidedEntity
    {
        public virtual CustomerEntity Customer { get; protected set; }
        public virtual DiscountGroupEntity DiscountGroup { get; protected set; }
        public virtual Discount Discount { get; protected set; }
        public virtual bool DiscountActive { get; protected set; }
        public virtual byte[] EnovaStamp { get; protected set; }

        protected CustomerDiscountEntity() { }

        public CustomerDiscountEntity(CustomerEntity customer, DiscountGroupEntity discountGroup, Discount discount, bool discountActive = true)
        {
            Guard.NotNull(customer, nameof(customer));
            Guard.NotNull(discountGroup, nameof(discountGroup));
            Guard.NotNull(discount, nameof(discount));

            Customer = customer;
            DiscountGroup = discountGroup;
            Discount = discount;
            DiscountActive = discountActive;

            EnovaStamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        public virtual void Deactivate()
        {
            DiscountActive = false;
        }

        public virtual void Activate()
        {
            DiscountActive = true;
        }

        public virtual void ToggleActive(bool active)
        {
            if (active)
                Activate();
            else
                Deactivate();
        }

        public virtual void SetDiscount(Discount discount)
        {
            if(Discount != discount)
            {
                Discount = discount;
            }
        }
    }
}
