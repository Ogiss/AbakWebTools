using AbakTools.Core.Framework.Domain;
using System;

namespace AbakTools.Core.Domain
{
    public class BusinessEntity : GuidedEntity, IBusinessEntity
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ModificationDate { get; set; }
        public virtual bool DisableUpdateModificationDate { get; set; }

        public virtual void Touch()
        {
            ModificationDate = DateTime.Now;
        }
    }
}
