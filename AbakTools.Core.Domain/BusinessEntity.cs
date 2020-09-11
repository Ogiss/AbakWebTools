using AbakTools.Core.Framework;
using System;

namespace AbakTools.Core.Domain
{
    public class BusinessEntity : GuidedEntity, IBusinessEntity
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ModificationDate { get; set; }
        public virtual bool DisableUpdateModificationDate { get; set; }
    }
}
