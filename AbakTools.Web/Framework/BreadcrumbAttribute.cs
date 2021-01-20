using System;

namespace AbakTools.Web.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BreadcrumbAttribute : Attribute
    {
        public string Title { get; }

        public BreadcrumbAttribute(string title) 
            => Title = title;
    }
}
