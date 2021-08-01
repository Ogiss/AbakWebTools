using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.DiscountGroup;
using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework;
using AbakTools.Core.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Core.Domain.Product
{
    public class ProductEntity : BusinessEntity, IStampedEntity, ISynchronizableEntity, IWebIdHolder
    {
        public virtual int? WebId { get; set; }
        public virtual TaxEntity Tax { get; set; }
        public virtual decimal Price { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual string SupplierCode { get; set; }
        public virtual bool Active { get; set; }
        public virtual float Quantity { get; set; }
        public virtual string Description { get; set; }
        public virtual string DescriptionShort { get; set; }
        public virtual string LinkRewrite { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string MetaTitle { get; set; }
        public virtual string MetaWords { get; set; }
        public virtual string Name { get; protected set; }
        public virtual Guid? EnovaGuid { get; protected set; }
        public virtual SynchronizeType Synchronize { get; set; }
        public virtual bool IsReady { get; set; }
        public virtual UnitEntity Unit { get; set; }
        public virtual bool IsEnovaProduct { get; set; }
        public virtual SupplierEntity Supplier { get; set; }
        public virtual bool IsAvailable { get; set; }
        public virtual int Order { get; set; }
        public virtual int FormOrder { get; set; }
        public virtual string OrderIndex { get; set; }
        public virtual string SearchIndex { get; set; }
        public virtual bool NotWebAvailable { get; set; }
        public virtual int MinimumOrderQuantity { get; set; }
        public virtual long Stamp { get; set; }
        public virtual bool OnSale { get; protected set; }

        public virtual bool IsArchived => IsDeleted || Synchronize == SynchronizeType.Deleted;

        public virtual ISet<CategoryEntity> Categories { get; set; } = new HashSet<CategoryEntity>();
        public virtual ISet<ImageEntity> Images { get; set; } = new HashSet<ImageEntity>();
        public virtual ISet<ProductDiscountGroupEntity> ProductDiscountGroups { get; set; } = new HashSet<ProductDiscountGroupEntity>();

        protected ProductEntity() { }

        public ProductEntity(string code, string name, Guid enovaGuid)
        {
            Guard.NotEmpty(code, nameof(code));
            Guard.NotEmpty(name, nameof(name));
            Guard.NotEmpty(enovaGuid, nameof(enovaGuid));

            Code = code;
            Name = name;
            EnovaGuid = enovaGuid;
            Synchronize = SynchronizeType.New;
        }

        public override void Touch()
        {
            if (Synchronize == SynchronizeType.Synchronized)
            {
                base.Touch();
                Synchronize = SynchronizeType.Edited;
            }
        }

        public virtual void MakeSynchronized()
        {
            if (Synchronize == SynchronizeType.Deleted)
            {
                IsDeleted = true;
            }

            IsReady = false;

            Synchronize = SynchronizeType.Synchronized;
        }

        public virtual IEnumerable<ImageEntity> GetUndeletedImages()
        {
            return Images.Where(x => x.IsDeleted == false && x.Synchronize != SynchronizeType.Deleted);
        }

        public virtual void ClearWebIdentity()
        {
            WebId = null;
        }

        public virtual void SetCode(string code)
        {
            Guard.NotEmpty(code, nameof(code));
            Code = code;
        }

        public virtual void SetName(string name)
        {
            Guard.NotEmpty(name, nameof(name));
            Name = name;

            if (Name.Length > 128)
            {
                Name = Name.Substring(0, 128);
            }
        }

        public virtual void SetPrice(decimal price)
        {
            if (Price != price)
            {
                Price = price;
            }
        }

        public virtual void StartDeletingProcess()
        {
            Synchronize = SynchronizeType.Deleted;
        }

        public virtual ProductDiscountGroupEntity AddDiscountGroup(DiscountGroupEntity discountGroup)
        {
            Guard.NotNull(discountGroup, nameof(discountGroup));

            var productGroup = ProductDiscountGroups.SingleOrDefault(x => x.DiscountGroup.Id == discountGroup.Id);

            if (productGroup == null)
            {
                productGroup = new ProductDiscountGroupEntity(this, discountGroup);
                ProductDiscountGroups.Add(productGroup);
            }

            return productGroup;
        }
    }
}
