using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain.Category
{
    public class CategoryEntity : BusinessEntity
    {
        public virtual CategoryEntity Parent { get; set; }
        public virtual byte? LevelDepth { get; set; }
        public virtual bool? Active { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string LinkRewrite { get; set; }
        public virtual string MetaTitle { get; set; }
        public virtual string MetaDescription { get; set; }
        public virtual string MetaKeywords { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual bool EnovaFeature { get; set; }
        public virtual int? WebId { get; set; }
        public virtual DateTime DateTimeStamp { get; set; }

        public virtual bool IsArchived => Synchronize == SynchronizeType.Deleted || IsDeleted;
    }
}
