using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain.Synchronize
{
    public interface ISynchronizeStampRepository : IGenericEntityRepository<SynchronizeStampEntity>
    {
        SynchronizeStampEntity Get(string code, SynchronizeDirectionType type);
    }
}
