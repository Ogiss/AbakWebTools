using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain.Supplier
{
    public interface ISupplierRepository : IGenericGuidedEntityRepository<SupplierEntity>
    {
        IReadOnlyCollection<SupplierEntity> GetAllModified(long stampFrom, long stampTo);
    }
}
