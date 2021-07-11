using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipies.Data;
using Recipies.Models.RecipesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    public class RecipesController : Controller
    {
        private readonly  RecipiesDbContext _recipesDbContext;
        private readonly IMapper _mapper;
        public RecipesController(RecipiesDbContext recipiesDbContext, IMapper mapper)
        {
            this._recipesDbContext = recipiesDbContext;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult All()
        {
            var recipes = this._recipesDbContext.Recipes.ToList();
            var models = _mapper.Map<RecipeViewModel>(recipes);
            return View(models);
        }
    }
}
