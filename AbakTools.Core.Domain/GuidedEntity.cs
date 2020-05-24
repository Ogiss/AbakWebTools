using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain
{
    public class GuidedEntity : Entity, IGuidedEntity
    {
        public virtual Guid Guid { get; set; }
    }
}
