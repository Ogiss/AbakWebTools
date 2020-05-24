using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Framework
{
    public interface IGuidedEntity : IEntity
    {
        public Guid Guid { get; set; }
    }
}
