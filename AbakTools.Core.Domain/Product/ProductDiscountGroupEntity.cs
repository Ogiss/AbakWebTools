using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Framework;
using System;

namespace AbakTools.Core.Domain.Product
{
    public class ProductDiscountGroupEntity
    {
        public virtual ProductEntity Product { get; protected set; }
        public virtual DiscountGroupEntity DiscountGroup { get; protected set; }
        public virtual bool IsDeleted { get; protected set; }
        public virtual SynchronizeType Synchronize { get; protected set; }

        public virtual bool IsArchived => IsDeleted || Synchronize == SynchronizeType.Deleted;

        protected ProductDiscountGroupEntity() { }

        public ProductDiscountGroupEntity(ProductEntity product, DiscountGroupEntity discountGroup)
        {
            Guard.NotNull(product, nameof(product));
            Guard.NotNull(discountGroup, nameof(discountGroup));

            Product = product;
            DiscountGroup = discountGroup;
            Synchronize = SynchronizeType.New;
        }

        public virtual void StartDeletingProcess()
        {
            if (!IsDeleted)
            {
                Synchronize = SynchronizeType.Deleted;
            }
        }

        public virtual void MakeSynchronized()
        {
            if (Synchronize == SynchronizeType.Deleted)
            {
                IsDeleted = true;
            }

            Synchronize = SynchronizeType.Synchronized;
        }

        public virtual void Restore()
        {
            if (IsArchived)
            {
                IsDeleted = false;
                
                if(Synchronize !=  SynchronizeType.New)
                {
                    Synchronize = SynchronizeType.Edited;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ProductDiscountGroupEntity entity)
            {
                return entity.Product == Product && entity.DiscountGroup == DiscountGroup;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Product.Id, DiscountGroup.Id);
        }
    }
}
