using AbakTools.Web.Models.Shared;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Web.ViewComponents.NavigationMenu
{
    public class NavigationMenuModelBuilder
    {
        private readonly List<NavigationMenuModelItemBuilder> itemBuilders = new List<NavigationMenuModelItemBuilder>();

        public NavigationMenuModelItemBuilder AddItem(string name)
        {
            var item = new NavigationMenuModelItemBuilder(name);
            itemBuilders.Add(item);

            return item;
        }

        public NavigationMenuModel Build()
        {
            return new NavigationMenuModel
            {
                MenuItems = itemBuilders.Select(x => x.Build()).Where(x => x != null).ToList()
            };
        }

    }
}
