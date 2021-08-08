using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
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
        public CategoryController(IRecipesService recipesService, ICategoryService categoryService)
        {
            _recipesService = recipesService;
            _categoryService = categoryService;
        }

        public async Task<ActionResult> FilterRecipes(string id)
        {
            ;
            return View();
        }
    }
}
