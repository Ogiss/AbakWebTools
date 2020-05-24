using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain
{
    public interface IGenericGuidedEntityRepository<TEntity> : IGenericEntityRepository<TEntity>
        where TEntity: IGuidedEntity
    {
        TEntity Get(Guid guid);
    }
}
