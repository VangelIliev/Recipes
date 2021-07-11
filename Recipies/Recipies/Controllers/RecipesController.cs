using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Models;
using Recipies.Data.Models.DbContext;
using Recipies.Models.RecipesModels;
using System.Collections.Generic;
using System.Linq;


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
            var models = _mapper.Map<List<RecipeModel>>(recipes);
            return View(models);
        }
    }
}
