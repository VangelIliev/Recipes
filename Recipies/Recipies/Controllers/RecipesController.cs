using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipes.Domain.Implementation;
using Recipes.Domain.Models;
using Recipies.Data.Models.DbContext;
using Recipies.Data.Models.Entities;
using Recipies.Models.RecipesModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Recipies.Controllers
{
    public class RecipesController : Controller
    {        
        private readonly IMapper _mapper;
        private readonly IRecipesService _recipeService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<IdentityUser> _userManager;
        public RecipesController( 
            IMapper mapper, 
            IRecipesService recipesService,
            ICategoryService categoryService,
            UserManager<IdentityUser> userManager)
        {          
            this._mapper = mapper;
            this._recipeService = recipesService;
            this._categoryService = categoryService;
            this._userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categoriesModels = _categoryService.FindAllAsync().Result;
            var categories = new List<string>();            
            foreach (var category in categoriesModels)
            {
                categories.Add(category.Name);
            }
            var recipeModel = new RecipeViewModel
            {
                Categories = categories
            };
            return View(recipeModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.CreatedOn = System.DateTime.UtcNow;           
            var user = await this._userManager.GetUserAsync(HttpContext.User);
            var userID = user.Id;
            model.ApplicationUserId = userID;
            model.CategoryId = model.CategoryId;
            model.NumberOfComments = 0;
            model.NumberOfLikes = 0;
            var recipeModel = _mapper.Map<RecipeModel>(model);
            await _recipeService.CreateAsync(recipeModel);

            return RedirectToAction("/Home/Index");
        }
    }
}
