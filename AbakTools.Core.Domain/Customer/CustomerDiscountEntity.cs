using AbakTools.Core.Domain.Common;
using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Customer
{
    public class CustomerDiscountEntity : GuidedEntity, IStampedEntity, ISynchronizableEntity
    {
        public virtual CustomerEntity Customer { get; protected set; }
        public virtual DiscountGroupEntity DiscountGroup { get; protected set; }
        public virtual Discount Discount { get; protected set; }
        public virtual bool DiscountActive { get; protected set; }
        public virtual long Stamp { get; protected set; }
        public virtual byte[] EnovaStamp { get; protected set; }
        public virtual SynchronizeType Synchronize { get; protected set; }

        protected CustomerDiscountEntity() { }

        public CustomerDiscountEntity(CustomerEntity customer, DiscountGroupEntity discountGroup, Discount discount, bool discountActive = true)
        {
            Guard.NotNull(customer, nameof(customer));
            Guard.NotNull(discountGroup, nameof(discountGroup));
            Guard.NotNull(discount, nameof(discount));

            Guid = discountGroup.Guid;
            Customer = customer;
            DiscountGroup = discountGroup;
            Discount = discount;
            DiscountActive = discountActive;
            Synchronize = SynchronizeType.New;

            EnovaStamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        public virtual void Deactivate()
        {
            if (DiscountActive)
            {
                DiscountActive = false;
                MakeEdited();
            }
        }

        public virtual void Activate()
        {
            if (!DiscountActive)
            {
                DiscountActive = true;
                MakeEdited();
            }
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
            if (Discount != discount)
            {
                Discount = discount;
                MakeEdited();
            }
        }

        public virtual void MakeEdited()
        {
            if (Synchronize == SynchronizeType.Synchronized)
            {
                Synchronize = SynchronizeType.Edited;
            }
        }

        public virtual void MakeSynchronized()
        {
            Synchronize = SynchronizeType.Synchronized;
        }
    }
}
