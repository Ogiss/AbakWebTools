using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Dto.Category
{
    public class GetCategoryResponseDto
    {
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public byte? LevelDepth { get; set; }
        public bool? Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LinkRewrite { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int DisplayOrder { get; set; }
    }
}
