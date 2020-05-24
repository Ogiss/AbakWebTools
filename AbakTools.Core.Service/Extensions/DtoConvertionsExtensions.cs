using AbakTools.Core.Domain.Category;
using AbakTools.Core.Dto.Category;
using AbakTools.Core.Framework;
using AutoMapper;
using System;

namespace AbakTools.Core.Service.Extensions
{
    public static class DtoConvertionsExtensions
    {
        private static Lazy<IMapper> mapper = new Lazy<IMapper>(() => {
            return DependencyProvider.Resolve<IMapper>();
        }, true);

        public static CategoryItemDto ToDto(this CategoryEntity x) => mapper.Value.Map<CategoryItemDto>(x);
    }
}
