using AbakTools.Web.Framework;
using Microsoft.AspNetCore.Mvc;

namespace AbakTools.Web.Controllers
{
    [Route("kartoteki/kategorie")]
    [Breadcrumb("Kartoteki")]
    public class CategoryManagementController : Controller
    {
        [Route("")]
        [Breadcrumb("Kategorie")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(nameof(GetCategoriesTree))]
        public IActionResult GetCategoriesTree()
        {
            return Json(new[] { 
                new
                {
                    Id = "root",
                    Text = "Root",
                    Children = new[]
                    {
                        new { Id = "child1", Text = "Child 1" },
                        new { Id = "child2", Text = "Child 2" },
                        new { Id = "child3", Text = "Child 3" },
                    }
                }
            });
        }
    }
}
