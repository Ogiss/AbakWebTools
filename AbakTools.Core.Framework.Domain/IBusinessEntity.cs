using System;

namespace AbakTools.Core.Framework.Domain
{
    public interface IBusinessEntity : IGuidedEntity
    {
        bool IsDeleted { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ModificationDate { get; set; }
        bool DisableUpdateModificationDate { get; }
    }
}
