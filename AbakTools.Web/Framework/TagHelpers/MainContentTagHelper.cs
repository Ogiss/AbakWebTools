using AbakTools.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace AbakTools.Web.Framework.TagHelpers
{

    [HtmlTargetElement("main-content", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MainContentTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly HtmlEncoder _htmlEncoder;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string PageTitle { get; set; }

        public string PageSubtitle { get; set; }

        public MainContentTagHelper(IHtmlHelper htmlHelper, HtmlEncoder htmlEncoder)
        {
            _htmlHelper = htmlHelper;
            _htmlEncoder = htmlEncoder;
        }

        public async override System.Threading.Tasks.Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            (_htmlHelper as IViewContextAware).Contextualize(ViewContext);

            output.TagName = "div";
            output.AddClass("main-content", _htmlEncoder);

            var pageHeader = await _htmlHelper.PartialAsync("_PageContentHeader", CreatePageContentHeaderModel());
            var childContent = await output.GetChildContentAsync();

            var container = new TagBuilder("div");
            container.AddCssClass("container-fluid");
            container.InnerHtml.AppendHtml(childContent);

            output.Content.AppendHtml(pageHeader);
            output.Content.AppendHtml(container);
        }

        private PageContentHeaderModel CreatePageContentHeaderModel()
        {
            return new PageContentHeaderModel
            {
                PageTitle = PageTitle,
                PageSubtitle = PageSubtitle
            };
        }
    }
}
