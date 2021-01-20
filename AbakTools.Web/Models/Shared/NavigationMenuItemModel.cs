using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Web.Models.Shared
{
    public class NavigationMenuItemModel
    {
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsActive { get; set; }
        public string IconClass { get; set; }
        public List<NavigationMenuItemModel> ChildItems { get; set; } = new List<NavigationMenuItemModel>();

        public bool HasChildren => ChildItems?.Any() ?? false;
        public bool HasIcon => !string.IsNullOrEmpty(IconClass);
    }
}
