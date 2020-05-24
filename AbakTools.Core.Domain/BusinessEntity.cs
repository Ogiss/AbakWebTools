using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain
{
    public class BusinessEntity : GuidedEntity, IBusinessEntity
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ModificationDate { get; set; }
    }
}
