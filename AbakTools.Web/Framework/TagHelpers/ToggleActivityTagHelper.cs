using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace AbakTools.Web.Framework.TagHelpers
{
    [HtmlTargetElement(Attributes = "toggle-activity")]
    public class ToggleActivityTagHelper : TagHelper
    {
        private readonly HtmlEncoder _htmlEncoder;

        public bool ToggleActivity { get; set; }

        public ToggleActivityTagHelper(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ToggleActivity)
            {
                output.AddClass("active", _htmlEncoder);
            }
        }
    }
}
