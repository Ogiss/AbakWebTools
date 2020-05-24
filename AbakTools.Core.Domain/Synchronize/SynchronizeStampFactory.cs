using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain.Synchronize
{
    public static class SynchronizeStampFactory
    {
        public static SynchronizeStampEntity Create(string code, SynchronizeDirectionType type)
        {
            Guard.NotEmpty(code, nameof(code));

            return new SynchronizeStampEntity(code, type);
        }
    }
}
