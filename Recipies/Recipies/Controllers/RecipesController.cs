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
using System;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult> All()
        {
            var recipesModels = await _recipeService.FindAllAsync();
            var recipeViewModels = new List<RecipeViewModel>();
            foreach (var model in recipesModels)
            {
                var recipe = await _recipeService.ReadAsync(Guid.Parse(model.Id));
                var userName = await _userManager.FindByIdAsync(recipe.ApplicationUserId);
                var recipeViewModel = new RecipeViewModel
                {
                    Id = recipe.Id,
                    PreparationDescription = recipe.PreparationDescription,
                    TimeToPrepare = recipe.TimeToPrepare,
                    ImageUrl = recipe.ImageUrl,
                    CreatedBy = userName.Email,
                    NumberOfComments = recipe.NumberOfComments,
                    NumberOfLikes = recipe.NumberOfLikes
                };
                recipeViewModels.Add(recipeViewModel);
            }
            return View(recipeViewModels);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Own()
        {
            var recipes = await _recipeService.FindAllAsync();
            var user = await this._userManager.GetUserAsync(HttpContext.User);
            var userID = user.Id;
            var recipesOfUser = recipes.Where(x => x.ApplicationUserId == userID).ToList();

            var recipeViewModels = _mapper.Map<List<RecipeViewModel>>(recipesOfUser);
            return View(recipeViewModels);
        }
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            var recipe = await _recipeService.ReadAsync(Guid.Parse(id));
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);
            return View(recipeViewModel);
        }
        [HttpGet]
        public async Task<ActionResult> RemoveRecipe(string id)
        {
            var recipe = await _recipeService.ReadAsync(Guid.Parse(id));
            await _recipeService.DeleteAsync(recipe);
            return this.Redirect("All");
        }

        [HttpGet]
        public async Task<ActionResult> UpdateRecipe(string id)
        {
            var recipe = await _recipeService.ReadAsync(Guid.Parse(id));            
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);                       
            return View(recipeViewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add()
        {
            var categoriesModels = await _categoryService.FindAllAsync();
                       
            var categoriesWithId = new Dictionary<string, string>();

            foreach (var category in categoriesModels.Distinct())
            {
                categoriesWithId.Add(category.Id.ToString(), category.Name);                
            }
            var recipeModel = new RecipeViewModel
            {
                Categories = categoriesWithId
            };
            return View(recipeModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(RecipeViewModel model)
        {
            var categoriesModels = await _categoryService.FindAllAsync();

            var categoriesWithId = new Dictionary<string, string>();
            
            foreach (var category in categoriesModels.Distinct())
            {
                categoriesWithId.Add(category.Id.ToString(), category.Name);

            }
            model.Categories = categoriesWithId;
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
            var recipeModelData = _mapper.Map<RecipeModel>(model);
            await _recipeService.CreateAsync(recipeModelData);

            return RedirectToAction("All");
        }
    }
}
