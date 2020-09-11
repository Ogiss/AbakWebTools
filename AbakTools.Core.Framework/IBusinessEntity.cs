using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Framework
{
    public interface IBusinessEntity : IGuidedEntity
    {
        bool IsDeleted { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ModificationDate { get; set; }
        bool DisableUpdateModificationDate { get; }
    }
}
