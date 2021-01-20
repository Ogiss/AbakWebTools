using AbakTools.Web.Models.Shared;
using AbakTools.Web.ViewComponents.NavigationMenu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Web.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var builder = new NavigationMenuModelBuilder();

            builder.AddItem("Dashboard")
                .ForController("Dashboard")
                .WithIconClass("ti-dashboard");

            var filesSection = builder.AddItem("Kartoteki")
                .WithIconClass("ti-files");

            filesSection.AddChildItem("Kategorie")
                .ForController("CategoryManagement");

            var model = builder.Build();
            SetActiveMenuItems(model.MenuItems);

            return View(model);
        }

        private void SetActiveMenuItems(List<NavigationMenuItemModel> menuItems)
        {
            if (ViewContext.ActionDescriptor is ControllerActionDescriptor actionDescription)
            {
                string controller = actionDescription.ControllerName;
                string action = actionDescription.ActionName;

                foreach (var item in menuItems)
                {
                    item.IsActive = item.Controller == controller;

                    foreach (var childItem in item.ChildItems)
                    {
                        childItem.IsActive = childItem.Controller == controller && childItem.Action == action;

                        if (childItem.IsActive)
                        {
                            item.IsActive = true;
                        }
                    }
                }
            }
        }
    }
}
