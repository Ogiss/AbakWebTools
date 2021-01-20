using AbakTools.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AbakTools.Web.Framework.TagHelpers
{
    [HtmlTargetElement("breadcrumbs", TagStructure = TagStructure.WithoutEndTag)]
    public class BreadcrumbsTagHelper : TagHelper
    {
        private const string DefaultActionName = "Index";
        private readonly IHtmlHelper _htmlHelper;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly LinkGenerator _linkGenerator;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public BreadcrumbsTagHelper(IHtmlHelper htmlHelper, HtmlEncoder htmlEncoder, LinkGenerator linkGenerator)
        {
            _htmlHelper = htmlHelper;
            _htmlEncoder = htmlEncoder;
            _linkGenerator = linkGenerator;
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            (_htmlHelper as IViewContextAware).Contextualize(ViewContext);

            var content = await _htmlHelper.PartialAsync("_Breadcrumbs", CreateModel());

            output.TagName = null;
            output.Content.AppendHtml(content);
        }

        private IEnumerable<BreadcrumbsItemModel> CreateModel()
        {
            var actionDescription = ViewContext.ActionDescriptor as ControllerActionDescriptor
               ?? throw new InvalidOperationException($"Cannot render breadcrumbs, {nameof(ControllerActionDescriptor)} is not available in current context.");

            string controller = actionDescription.ControllerName;
            string action = actionDescription.ActionName;
            string prefixTitle = GetControllerBreadcrumbPrefix(actionDescription, controller);

            var breadcrumbs = new List<BreadcrumbsItemModel>
            {
                new BreadcrumbsItemModel(prefixTitle)
            };

            if (action != DefaultActionName)
            {
                var indexActionTitle = GetControllerActionBreadcrumbTitle(actionDescription, DefaultActionName);
                if (indexActionTitle != null)
                {
                    breadcrumbs.Add(new BreadcrumbsItemModel(indexActionTitle, _linkGenerator.GetPathByAction(DefaultActionName, controller)));
                }
            }

            var actionTitle = GetControllerActionBreadcrumbTitle(actionDescription, action);
            if (actionTitle != null)
            {
                breadcrumbs.Add(new BreadcrumbsItemModel(actionTitle));
            }

            return breadcrumbs;
        }

        private static string GetControllerBreadcrumbPrefix(ControllerActionDescriptor actionDescriptor, string controllerName)
        {
            var attribute = actionDescriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(BreadcrumbAttribute), true)
                .Cast<BreadcrumbAttribute>()
                .FirstOrDefault();

            return attribute?.Title ?? controllerName;
        }

        private static string GetControllerActionBreadcrumbTitle(ControllerActionDescriptor actionDescriptor, string actionName)
        {
            try
            {
                var methodInfo = actionDescriptor.ControllerTypeInfo.GetMethod(actionName);

                if (methodInfo != null)
                {
                    var attribute = methodInfo.GetCustomAttributes(typeof(BreadcrumbAttribute), true)
                        .Cast<BreadcrumbAttribute>()
                        .FirstOrDefault();

                    return attribute?.Title;
                }
                else
                {
                    return null;
                }
            }
            catch (AmbiguousMatchException)
            {
                return actionName;
            }
        }
    }
}
