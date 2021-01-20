using AbakTools.Web.Models.Shared;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Web.ViewComponents.NavigationMenu
{
    public class NavigationMenuModelItemBuilder
    {
        private readonly string name;
        private string controller;
        private string action = "Index";
        private string iconClass = "ti-file";
        private readonly List<NavigationMenuModelItemBuilder> childItemBuilders = new List<NavigationMenuModelItemBuilder>();

        public NavigationMenuModelItemBuilder(string name)
        {
            this.name = name;
        }

        public NavigationMenuModelItemBuilder ForController(string controller)
        {
            this.controller = controller;

            return this;
        }

        public NavigationMenuModelItemBuilder ForAction(string action)
        {
            this.action = action;

            return this;
        }

        public NavigationMenuModelItemBuilder WithIconClass(string iconClass)
        {
            this.iconClass = iconClass;

            return this;
        }

        public NavigationMenuModelItemBuilder AddChildItem(string name)
        {
            var item = new NavigationMenuModelItemBuilder(name);
            childItemBuilders.Add(item);

            return item;
        }

        public NavigationMenuItemModel Build()
        {

            var childItems = childItemBuilders
                .Select(x => x.Build())
                .Where(x => x != null)
                .ToList();

            if (controller != null || childItems.Any())
            {
                return new NavigationMenuItemModel
                {
                    Name = name,
                    Controller = controller,
                    Action = action,
                    IconClass = iconClass,
                    ChildItems = childItems
                };
            }
            else
            {
                return null;
            }
        }
    }
}
