using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;

        public AdminController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult> AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory()
        {
            return View();
        }
    }
}
