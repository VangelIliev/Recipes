using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    [Authorize]
    public class LikesController : Controller
    {
        private readonly ILikeService _likeService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRecipesService _recipesService;
        public LikesController(
            ILikeService likeService, 
            UserManager<IdentityUser> userManager,
            IRecipesService recipesService)
        {
            this._likeService = likeService;
            this._userManager = userManager;
            this._recipesService = recipesService;
        }
        [HttpPost]
        public async Task<ActionResult> LikeRecipe(string id)
        {
            var likes = await _likeService.FindAllAsync();
            var likesForCurrentRecipe = likes.Where(x => x.RecipeId == id);
            var user = await this._userManager.GetUserAsync(HttpContext.User);
            var userID = user.Id;
            var isRecipeLikedByUser = likesForCurrentRecipe.Any(x => x.ApplicationUserId == userID);
            if(isRecipeLikedByUser)
            {
                return Json(new { success = false, message = "You have already liked this recipe !!" });
            }
            var likeModel = new LikeModel
            {               
                RecipeId = id,
                ApplicationUserId = userID,
            };

            await this._likeService.CreateAsync(likeModel);
            var recipe = await this._recipesService.ReadAsync(Guid.Parse(id));
            recipe.NumberOfLikes++;
            await this._recipesService.UpdateAsync(recipe);
            return Json(new {success = true, id = id });
        }

        [HttpPost]
        public async Task<ActionResult> DisLikeRecipe(string id)
        {
            var likes = await _likeService.FindAllAsync();
            var likesForCurrentRecipe = likes.Where(x => x.RecipeId == id);
            var user = await this._userManager.GetUserAsync(HttpContext.User);
            var userID = user.Id;
            var isRecipeLikedByUser = likesForCurrentRecipe.Any(x => x.ApplicationUserId == userID);
            if (isRecipeLikedByUser == false)
            {
                return Json(new { success = false, message = "You cannot dislike the recipe you can only remove your like!" });
            }
            var likedRecipeByUser = likesForCurrentRecipe.FirstOrDefault(x => x.ApplicationUserId == userID);

            await this._likeService.DeleteAsync(likedRecipeByUser);
            var recipe = await this._recipesService.ReadAsync(Guid.Parse(id));
            recipe.NumberOfLikes--;
            await this._recipesService.UpdateAsync(recipe);
            return Json(new { success = true, id = id });
        }

    }
}
