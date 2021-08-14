using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Models.AdminModels;
using Recipies.Models.RecipesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRecipesService _recipesService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(
            IRecipesService recipesService, 
            ICategoryService categoryService, 
            IMapper mapper)
        {
            _recipesService = recipesService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Filter(string id)
        {
            var recipes = await _recipesService.FindAllAsync();
            var filteredRecipes = recipes.Where(x => x.CategoryId == Guid.Parse(id));
            var recipesViewModel = _mapper.Map<List<RecipeViewModel>>(filteredRecipes);
            return View(recipesViewModel);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            
            return View("AddCategory");
        }

        [HttpPost]

        public async Task<ActionResult> AddCategory(AddCategoryViewModel model)
        {
            var categoryModel = _mapper.Map<CategoryModel>(model);
            await _categoryService.CreateAsync(categoryModel);
            return Redirect("/Recipes/All");
        }

        [HttpGet]
        public async Task<ActionResult> RemoveCategory()
        {
            var categories = await _categoryService.FindAllAsync();            
            var selectList = new Dictionary<string,string>();
            foreach (var category in categories)
            {
                selectList.Add(category.Name,category.Id.ToString());
            }
            var removeCategoryModel = new RemoveCategoryModel
            {
                Categories = selectList
            };
            return View(removeCategoryModel);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveCategory(RemoveCategoryModel model)
        {
            var category = await _categoryService.ReadAsync(Guid.Parse(model.Id));
            await _categoryService.DeleteAsync(category);
            return RedirectToAction("All","Recipes");
        }
    }
}
