using System.Threading.Tasks;
using AbakTools.Core.Framework.UnitOfWork;
using AbakTools.Core.Service.Category;
using Microsoft.AspNetCore.Mvc;

namespace AbakTools.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IUnitOfWorkProvider _unitOfWorkProvider;
        private ICategoryService _categoryService;

        public CategoryController(IUnitOfWorkProvider unitOfWorkProvider, ICategoryService categoryService)
        {
            _unitOfWorkProvider = unitOfWorkProvider;
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll(int? parentId)
        {
            using (var uow = _unitOfWorkProvider.CreateReadOnly())
            {
                return Ok(_categoryService.GetAll(parentId));
            }
        }

        [HttpGet("Get")]
        public IActionResult Get(int id)
        {
            using (var uow = _unitOfWorkProvider.CreateReadOnly())
            {
                return Ok(_categoryService.Get(id));
            }
        }
    }
}