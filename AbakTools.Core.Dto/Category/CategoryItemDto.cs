using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AbakTools.Core.Dto.Category
{
    public class CategoryItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("displayorder")]
        public int DisplayOrder { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("leveldepth")]
        public byte? LevelDepth { get; set; }
    }
}
