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
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly IRecipeDislikesService _recipeDislikesService;
        private readonly IProductService _productService;
        private readonly IRecipeProductsService _recipeProductsService;
        
        public RecipesController(
            IMapper mapper,
            IRecipesService recipesService,
            ICategoryService categoryService,
            UserManager<IdentityUser> userManager,
            ILikeService likeService,
            ICommentService commentService,
            IRecipeDislikesService recipeDislikesService,
            IProductService productService, IRecipeProductsService recipeProductsService)
        {
            this._mapper = mapper;
            this._recipeService = recipesService;
            this._categoryService = categoryService;
            this._userManager = userManager;
            this._likeService = likeService;
            this._commentService = commentService;
            this._recipeDislikesService = recipeDislikesService;
            this._productService = productService;
            this._recipeProductsService = recipeProductsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> All()
        {
            var recipesModels = await _recipeService.FindAllAsync();
            var recipeViewModels = new List<RecipeViewModel>();
            foreach (var model in recipesModels)
            {
                var recipe = await _recipeService.ReadAsync(Guid.Parse(model.Id));
                var userName = await _userManager.FindByIdAsync(recipe.ApplicationUserId);
                //All likes
                var allLlikesOfRecipes = await _likeService.FindAllAsync();
                var currentRecipeLikes = allLlikesOfRecipes.Where(x => x.RecipeId == Guid.Parse(model.Id)).ToList();
                var currentRecipeLikesCount = currentRecipeLikes.Count();

                //All Dislikes
                var allDisLlikesOfRecipes = await _recipeDislikesService.FindAllAsync();
                var currentRecipeDisLikes = allDisLlikesOfRecipes.Where(x => x.RecipeId == Guid.Parse(model.Id)).ToList();
                var currentRecipeDisLikesCount = currentRecipeDisLikes.Count();

                var numberOfLikes = currentRecipeLikesCount - currentRecipeDisLikesCount;
                var currentRecipeComments = await _commentService.FindAllAsync();
                var currentRecipeCommentsCount = currentRecipeComments.Where(x => x.RecipeId == model.Id).ToList().Count();
                var recipeViewModel = new RecipeViewModel
                {
                    Id = recipe.Id,
                    PreparationDescription = recipe.PreparationDescription,
                    TimeToPrepare = recipe.TimeToPrepare,
                    ImageUrl = recipe.ImageUrl,
                    CreatedBy = userName.Email,
                    NumberOfComments = recipe.NumberOfComments,
                    NumberOfLikes = numberOfLikes,
                    Name = recipe.Name
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
        [Authorize]
        public async Task<ActionResult> Details(string id)
        {
            var recipe = await _recipeService.ReadAsync(Guid.Parse(id));
            var categories = await _categoryService.FindAllAsync();
            var categoryForRecipe = categories.FirstOrDefault(x => x.Id == recipe.CategoryId);
            var recipeLikes = await _likeService.FindAllAsync();
            var likesCount = recipeLikes.Where(x => x.RecipeId == Guid.Parse(id)).Count();
            var recipeIngredients = await _recipeProductsService.FindAllAsync();
            var recipeIngredientsForCurrentRecipe = recipeIngredients.Where(x => x.RecipeId == Guid.Parse(recipe.Id)).ToList();
            var recipeIngredientsList = new List<RecipeIngredientInputModel>();
            foreach (var recipeIngredient in recipeIngredientsForCurrentRecipe)
            {
                var ingredient = await _productService.ReadAsync(recipeIngredient.ProductId);
                recipeIngredientsList.Add(new RecipeIngredientInputModel
                {
                    IngredientName = ingredient.Name,
                    Quantity = recipeIngredient.Quantity
                });
            }
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);
            recipeViewModel.Ingredients = recipeIngredientsList;
            recipeViewModel.NumberOfLikes = likesCount;
            recipeViewModel.Category = categoryForRecipe.Name;
            return View(recipeViewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> RemoveRecipe(string id) 
        {           
            var recipe = await _recipeService.ReadAsync(Guid.Parse(id));
            var ricipeId = new Guid(id.ToUpper());
            await _recipeService.DeleteAsync(ricipeId);
            return this.RedirectToAction("All","Recipes");                                
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> UpdateRecipe(string id)
        {
            var categoriesModels = await _categoryService.FindAllAsync();

            var categoriesWithId = new Dictionary<string, string>();

            foreach (var category in categoriesModels.Distinct())
            {
                categoriesWithId.Add(category.Id.ToString(), category.Name);
            }
            
            var recipe = await _recipeService.ReadAsync(Guid.Parse(id));
            var recipeViewModel = new RecipeViewModel
            {
                Id = recipe.Id,
                Categories = categoriesWithId,
                ImageUrl = recipe.ImageUrl,
                PreparationDescription = recipe.PreparationDescription,
                TimeToPrepare = recipe.TimeToPrepare,
                PortionsSize = recipe.PortionsSize
            };
            recipeViewModel.Categories = categoriesWithId;
            return View("Update", recipeViewModel);
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
            var productsIds = new List<Guid>();
            foreach (var inputIngredient in model.Ingredients)
            {
                var ingredients = await this._productService.FindAllAsync();
                var ingredient = ingredients.FirstOrDefault(x => x.Name == inputIngredient.IngredientName);
                if (ingredient == null)
                {
                    ingredient = new ProductModel
                    {
                        Name = inputIngredient.IngredientName,
                    };
                    var id = await this._productService.CreateAsync(ingredient);
                    productsIds.Add(id);
                }
                productsIds.Add(ingredient.Id);
            }
            var recipeModelData = _mapper.Map<RecipeModel>(model);
             
            var recipeId = await _recipeService.CreateAsync(recipeModelData);
            var counter = 0;
            for (int i = counter; i < model.Ingredients.Count;)
            {
                if (counter == model.Ingredients.Count)
                {
                    break;
                }
                foreach (var product in productsIds)
                {
                    var recipeProduct = new RecipeProductsModel
                    {
                        RecipeId = recipeId,
                        ProductId = product,
                        Quantity = model.Ingredients[counter].Quantity
                    };
                    await this._recipeProductsService.CreateAsync(recipeProduct);
                    counter++;
                    break;
                    
                }
            }                                     
            return RedirectToAction("All");
        }
    }
}
