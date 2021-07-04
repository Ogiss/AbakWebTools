using System;

namespace AbakTools.Core.Framework.Domain
{
    public interface IGuidedEntity : IEntity
    {
        public Guid Guid { get; set; }
    }
}
