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
        private readonly IRecipeDislikesService _recipeDislikesService;
        public LikesController(
            ILikeService likeService, 
            UserManager<IdentityUser> userManager,
            IRecipesService recipesService,
            IRecipeDislikesService recipeDislikesService)
        {
            this._likeService = likeService;
            this._userManager = userManager;
            this._recipesService = recipesService;
            this._recipeDislikesService = recipeDislikesService;
        }
        [HttpPost]
        public async Task<ActionResult> LikeRecipe(string id)
        {
            try
            {
                var likes = await _likeService.FindAllAsync();
                var likesForCurrentRecipe = likes.Where(x => x.RecipeId == Guid.Parse(id)).ToList();
                var recipe = await this._recipesService.ReadAsync(Guid.Parse(id));
                var dislikes = await _recipeDislikesService.FindAllAsync();
                var user = await this._userManager.GetUserAsync(HttpContext.User);
                var userID = user.Id;
                var isRecipeLikedByUser = likesForCurrentRecipe.Any(x => x.ApplicationUserId == userID);
                if (isRecipeLikedByUser)
                {
                    return Json(new { success = false, message = "You have already liked this recipe !!" });
                }
                var isDislikedByUser = dislikes.Any(x => x.ApplicationUserId == userID);

                if (isDislikedByUser)
                {
                    var dislikesForCurrentRecipe = dislikes.Where(x => x.RecipeId == Guid.Parse(id)).ToList();
                    var dislikesWithUser = dislikesForCurrentRecipe.FirstOrDefault(x => x.ApplicationUserId == userID);
                    await this._recipeDislikesService.DeleteAsync(dislikesWithUser);
                    return Json(new { success = true, id = id });
                }
                var likeModel = new LikeModel
                {
                    RecipeId = Guid.Parse(id),
                    ApplicationUserId = userID,
                };

                await this._likeService.CreateAsync(likeModel);                                                                
                return Json(new { success = true, id = id });
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> DisLikeRecipe(string id)
        {
            try
            {
                var dislikes = await _recipeDislikesService.FindAllAsync();
                var dislikesForCurrentRecipe = dislikes.Where(x => x.RecipeId == Guid.Parse(id)).ToList();
                var recipe = await _recipesService.ReadAsync(Guid.Parse(id));

                var likes = await _likeService.FindAllAsync();
                var user = await this._userManager.GetUserAsync(HttpContext.User);
                var userID = user.Id;
                var isRecipeDislikes = dislikesForCurrentRecipe.Any(x => x.ApplicationUserId == userID);
                if (isRecipeDislikes)
                {
                    return Json(new { success = false, message = "You have already disliked this recipe" });
                }
                var isLikedRecipe = likes.Any(x => x.ApplicationUserId == userID);
                if (isLikedRecipe)
                {
                    var likesForCurrentRecipe = likes.Where(x => x.RecipeId == Guid.Parse(id)).ToList();
                    var likesWithUser = likesForCurrentRecipe.FirstOrDefault(x => x.ApplicationUserId == userID);
                    await this._likeService.DeleteAsync(likesWithUser);                    
                    return Json(new { success = true, id = id });
                }
                var dislikedRecipe = new RecipeDislikesModel
                {
                    ApplicationUserId = userID,
                    RecipeId = Guid.Parse(id)
                };
                await _recipeDislikesService.CreateAsync(dislikedRecipe);                                                              
                return Json(new { success = true, id = id });
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

    }
}
