using AbakTools.Core.Domain.Category;
using AbakTools.Core.Dto.Category;
using AutoMapper;

namespace AbakTools.Core.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Category

            CreateMap<CategoryEntity, CategoryItemDto>();
            CreateMap<CategoryEntity, GetCategoryResponseDto>()
                .ForMember(d => d.ParentId, o => o.MapFrom(s => s.Parent.Id))
                .ForMember(d => d.ParentName, o => o.MapFrom(s => s.Parent.Name))
                .ForAllMembers(o => o.Condition((s, d, value) => value != null));
        }
    }
}
