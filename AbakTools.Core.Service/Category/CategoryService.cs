using AbakTools.Core.Service.Extensions;
using AbakTools.Core.Domain.Category;
using AbakTools.Core.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AbakTools.Core.Framework.Exeptions;

namespace AbakTools.Core.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public GetCategoryResponseDto Get(int id)
        {
            var category = _categoryRepository.Get(id);

            DomainException.ThrowIf(category == null, DomainExeptionCode.CategoryNotFound);

            var response = _mapper.Map<GetCategoryResponseDto>(category);

            return response;
        }

        public IReadOnlyList<CategoryItemDto> GetAll(int? parentId)
        {
            return _categoryRepository.GetAll(parentId)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .Select(x => _mapper.Map<CategoryItemDto>(x))
                .ToList();
        }
    }
}
