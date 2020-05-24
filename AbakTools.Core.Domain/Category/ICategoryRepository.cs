using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain.Category
{
    public interface ICategoryRepository : IGenericGuidedEntityRepository<CategoryEntity>
    {
        IReadOnlyList<CategoryEntity> GetAll(int? parentId);
        IReadOnlyCollection<CategoryEntity> GetAllModified(DateTime from, DateTime? to);
    }
}
