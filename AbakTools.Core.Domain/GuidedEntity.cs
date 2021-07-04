using AbakTools.Core.Framework.Domain;
using System;

namespace AbakTools.Core.Domain
{
    public class GuidedEntity : Entity, IGuidedEntity
    {
        public virtual Guid Guid { get; set; }
    }
}
