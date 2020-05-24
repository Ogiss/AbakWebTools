using AbakTools.Core.Dto.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Service.Category
{
    public interface ICategoryService
    {
        IReadOnlyList<CategoryItemDto> GetAll(int? parentId);
        GetCategoryResponseDto Get(int id);
    }
}
