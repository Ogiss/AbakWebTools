using System;

namespace AbakTools.Core.Framework.Domain
{
    public enum SynchronizeType
    {
        Synchronized = 0,

        [Obsolete("TO REMOVE")]
        Notsaved = 1,

        New = 2,

        Edited = 3,

        Deleted = 4,

        [Obsolete("TO REMOVE")]
        Synchronizing = 5
    }
}
