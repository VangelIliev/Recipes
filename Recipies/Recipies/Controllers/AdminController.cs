using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Models.AdminModels;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public AdminController(ICategoryService categoryService, IMapper mapper)
        {
            this._categoryService = categoryService;
            this._mapper = mapper;
        }
        [HttpGet]
        
        public IActionResult AddCategory()
        {
            AddCategoryViewModel model = new AddCategoryViewModel();
            return View("AddCategory",model);
        }

        [HttpPost]
        
        public async Task<ActionResult> AddCategory(AddCategoryViewModel model)
        {
            var categoryModel = _mapper.Map<CategoryModel>(model);
            await _categoryService.CreateAsync(categoryModel);
            return Redirect("/Recipes/All");
        }
    }
}
