using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain.Supplier
{
    public class SupplierEntity : GuidedEntity
    {
        public virtual string Name { get; set; }
        public virtual long Stamp { get; set; }
    }
}
