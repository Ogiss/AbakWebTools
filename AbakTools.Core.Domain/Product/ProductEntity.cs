using AbakTools.Core.Domain.Category;
using AbakTools.Core.Domain.Supplier;
using AbakTools.Core.Domain.Tax;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbakTools.Core.Domain.Product
{
    public class ProductEntity : BusinessEntity
    {
        public virtual int? WebId { get; set; }
        public virtual TaxEntity Tax { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Code { get; set; }
        public virtual string SupplierCode { get; set; }
        public virtual bool Active { get; set; }
        public virtual float Quantity { get; set; }
        public virtual string Description { get; set; }
        public virtual string DescriptionShort { get; set; }
        public virtual string LinkRewrite { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string MetaTitle { get; set; }
        public virtual string MetaWords { get; set; }
        public virtual string Name { get; set; }
        public virtual Guid? EnovaGuid { get; set; }
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

        public virtual ISet<CategoryEntity> Categories { get; set; } = new HashSet<CategoryEntity>();
        public virtual ISet<ImageEntity> Images { get; set; } = new HashSet<ImageEntity>();

        public virtual IEnumerable<ImageEntity> GetUndeletedImages()
        {
            return Images.Where(x => x.IsDeleted == false && x.Synchronize != SynchronizeType.Deleted);
        }
    }
}
