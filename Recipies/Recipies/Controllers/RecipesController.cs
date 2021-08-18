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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;
        
        public RecipesController(
            IMapper mapper,
            IRecipesService recipesService,
            ICategoryService categoryService,
            UserManager<IdentityUser> userManager,
            ILikeService likeService,
            ICommentService commentService,
            IRecipeDislikesService recipeDislikesService,
            IProductService productService, 
            IRecipeProductsService recipeProductsService,
            IImageService imageService,
            IWebHostEnvironment webHostEnvironment)
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
            this._imageService = imageService;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> All()
        {
            try
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
                        CreatedBy = userName.Email,
                        NumberOfComments = currentRecipeCommentsCount,
                        NumberOfLikes = numberOfLikes,
                        Name = recipe.Name
                    };
                    var imagesPaths = await _imageService.PopulateRecipeViewModelImages(Guid.Parse(recipe.Id));
                    recipeViewModel.ImagesFilePaths = imagesPaths;
                    recipeViewModels.Add(recipeViewModel);
                    
                }
                return View(recipeViewModels);
            }
            catch (Exception e)
            {

                return RedirectToAction("CustomError","Errors");
            }
            
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
            var recipeViewModelsList = new List<RecipeViewModel>();
            foreach (var model in recipeViewModels)
            {
                var imagesPaths = await _imageService.PopulateRecipeViewModelImages(Guid.Parse(model.Id));
                model.ImagesFilePaths = imagesPaths;                
                recipeViewModelsList.Add(model);
            }
            return View(recipeViewModelsList);
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

            var imagesPaths = await _imageService.PopulateRecipeViewModelImages(Guid.Parse(recipe.Id));
            recipeViewModel.ImagesFilePaths = imagesPaths;
            recipeViewModel.Ingredients = recipeIngredientsList;
            recipeViewModel.NumberOfLikes = likesCount;
            recipeViewModel.Category = categoryForRecipe.Name;
            return View(recipeViewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> RemoveRecipe(string id) 
        {                       
            var ricipeId = new Guid(id.ToUpper());
            await _recipeService.DeleteAsync(ricipeId);
            this.TempData["Message"] = "Successfully removed recipe !";
            return this.RedirectToAction("All","Recipes");                                
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> UpdateRecipe(string id)
        {
            try
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
                    Name = recipe.Name,
                    Categories = categoriesWithId,
                    PreparationDescription = recipe.PreparationDescription,
                    TimeToPrepare = recipe.TimeToPrepare,
                    PortionsSize = recipe.PortionsSize,
                    CategoryId = recipe.CategoryId.ToString()
                };
                recipeViewModel.Categories = categoriesWithId;
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
                var imagesPaths = await _imageService.PopulateRecipeViewModelImages(Guid.Parse(recipe.Id));
                recipeViewModel.ImagesFilePaths = imagesPaths;
                recipeViewModel.Ingredients = recipeIngredientsList;
                return View("Update", recipeViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("CustomError", "Errors");
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UpdateRecipe(RecipeViewModel model)
        {
            try
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
                    return View("Update",model);
                }

                var user = await this._userManager.GetUserAsync(HttpContext.User);
                var userID = user.Id;
                var recipeId = model.Id;
                //Update recipe entity
                var recipeInDb = await _recipeService.ReadAsync(Guid.Parse(model.Id));
                var recipeModel = _mapper.Map<RecipeModel>(model);
                recipeModel.ApplicationUserId = recipeInDb.ApplicationUserId;
                recipeModel.CreatedOn = recipeInDb.CreatedOn;
                await _recipeService.UpdateAsync(recipeModel);

                //update recipe products
                var recipeProducts = await _recipeProductsService.FindAllAsync();
                var recipeProductsForThisRecipe = recipeProducts.Where(x => x.RecipeId == Guid.Parse(model.Id)).ToList();
                foreach (var product in recipeProductsForThisRecipe)
                {
                    await _recipeProductsService.DeleteAsync(product);
                }
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
                    if(ingredient.Id != Guid.Empty)
                    {                        
                        productsIds.Add(ingredient.Id);
                    }
                    
                }
                var counter = 0;
                foreach (var item in productsIds)
                {
                    var recipeProductsDb = await _recipeProductsService.FindAllAsync();
                    var recipeProduct = recipeProductsDb.FirstOrDefault(x => x.ProductId == item);
                    if (recipeProduct != null)
                    {
                        await _recipeProductsService.DeleteAsync(recipeProduct);
                    }
                    
                }
                for (int j = 0; j < productsIds.Count;)
                {
                    if (counter == productsIds.Count)
                    {
                        break;
                    }
                    var recipeProduct = new RecipeProductsModel
                    {
                        RecipeId = Guid.Parse(recipeId),
                        ProductId = productsIds[counter],
                        Quantity = model.Ingredients[counter].Quantity
                    };
                    await this._recipeProductsService.CreateAsync(recipeProduct);
                    counter++;
                    
                }

                //Update Images
                var images = await _imageService.FindAllAsync();
                var imagesForRecipe = images.Where(x => x.RecipeId == Guid.Parse(recipeId)).ToList();
                foreach (var image in imagesForRecipe)
                {
                    await _imageService.DeleteAsync(image);
                    var path = image.FilePath;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);

                    }
                    
                }
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (model.Images != null)
                {
                    foreach (var image in model.Images)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(image.FileName + recipeId);
                        string extension = Path.GetExtension(image.FileName);

                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        var imageModel = new ImageModel
                        {
                            CreatedOn = DateTime.Now,
                            Extension = extension,
                            RecipeId = Guid.Parse(recipeId),
                            UserId = userID,
                            FilePath = path,
                            ImageName = fileName
                        };
                        await _imageService.CreateAsync(imageModel);

                    }
                }
                
                this.TempData["Message"] = "Successfully updated recipe !";
                return RedirectToAction("All", "Recipes");
            }
            catch (Exception e)
            {
                return View("Update",model);
            }
            
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
            try
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
                var user = await this._userManager.GetUserAsync(HttpContext.User);
                var userID = user.Id;
                model.CreatedOn = DateTime.Now;
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
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                foreach (var image in model.Images)
                {
                    string fileName = Path.GetFileNameWithoutExtension(image.FileName + recipeId);
                    string extension = Path.GetExtension(image.FileName);

                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    var imageModel = new ImageModel
                    {
                        CreatedOn = DateTime.Now,
                        Extension = extension,
                        RecipeId = recipeId,
                        UserId = userID,
                        FilePath = path,
                        ImageName = fileName
                    };
                    await _imageService.CreateAsync(imageModel);

                }
                var counter = 0;
                for (int i = counter; i < model.Ingredients.Count;)
                {
                    if (counter == model.Ingredients.Count)
                    {
                        break;
                    }
                    for (int j = 0; j < productsIds.Count; j++)
                    {
                        var recipeProduct = new RecipeProductsModel
                        {
                            RecipeId = recipeId,
                            ProductId = productsIds[counter],
                            Quantity = model.Ingredients[counter].Quantity
                        };
                        await this._recipeProductsService.CreateAsync(recipeProduct);
                        counter++;
                        break;
                    }
                    
                }
                this.TempData["Message"] = "Successfully added recipe !";
                return RedirectToAction("All");
            }
            catch (Exception е)
            {
                return RedirectToAction("CustomError", "Errors");
            }
            
        }

    }
}
