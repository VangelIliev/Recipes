using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipes.Domain.Implementation;
using Recipes.Domain.Models;
using Recipies.Data.Models.DbContext;
using Recipies.Models.RecipesModels;
using System.Collections.Generic;
using System.Linq;


namespace Recipies.Controllers
{
    public class RecipesController : Controller
    {        
        private readonly IMapper _mapper;
        private readonly IRecipesService _recipeService;
        public RecipesController(RecipiesDbContext recipiesDbContext, IMapper mapper, IRecipesService recipesService)
        {          
            this._mapper = mapper;
            this._recipeService = recipesService;
        }

        [HttpGet]
        public IActionResult Add()
        {
             
            return View();
        }

        [HttpPost]
        public IActionResult Add(RecipeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var recipeModel = _mapper.Map<RecipeModel>(model);
            _recipeService.CreateAsync(recipeModel);

            return RedirectToAction("/Home/Index");
        }
    }
}
