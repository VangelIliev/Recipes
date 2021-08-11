using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
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
    }
}
