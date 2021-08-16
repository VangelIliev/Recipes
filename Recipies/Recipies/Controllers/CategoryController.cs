using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly IImageService _imageService;
        private readonly ILikeService _likeService;
        private readonly IRecipeDislikesService _recipeDislikesService;
        private readonly ICommentService _commentService;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(
            IRecipesService recipesService,
            ICategoryService categoryService,
            IMapper mapper,
            IImageService imageService,
            ILikeService likeService,
            IRecipeDislikesService recipeDislikesService,
            ICommentService commentService, 
            UserManager<IdentityUser> userManager)
        {
            _recipesService = recipesService;
            _categoryService = categoryService;
            _mapper = mapper;
            _imageService = imageService;
            _likeService = likeService;
            _recipeDislikesService = recipeDislikesService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<ActionResult> Filter(string id)
        {
            var recipesModels = await _recipesService.FindAllAsync();
            var recipeViewModels = new List<RecipeViewModel>();
            foreach (var model in recipesModels.Where(x => x.CategoryId == Guid.Parse(id)).ToList())
            {
                var recipe = await _recipesService.ReadAsync(Guid.Parse(model.Id));
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
                    NumberOfComments = recipe.NumberOfComments,
                    NumberOfLikes = numberOfLikes,
                    Name = recipe.Name
                };
                var recipeModell = await PopulateRecipeViewModelImages(recipeViewModel, Guid.Parse(recipe.Id));
                recipeViewModels.Add(recipeModell);
            }
            return View(recipeViewModels);

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

        private async Task<RecipeViewModel> PopulateRecipeViewModelImages(RecipeViewModel model, Guid recipeId)
        {
            var images = await _imageService.FindAllAsync();
            var imagesForRecipe = images.Where(x => x.RecipeId == recipeId).ToList();
            model.ImagesFilePaths = new List<string>();
            foreach (var image in imagesForRecipe)
            {
                model.ImagesFilePaths.Add(image.ImageName);
            }

            return model;
        }
    }
}
