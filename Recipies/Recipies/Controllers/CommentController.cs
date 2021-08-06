using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Models.AdminModels;
using Recipies.Models.CommentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IRecipesService _recipesService;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICommentService _commentService;

        public CommentController(
            IRecipesService recipesService,
            IMapper mapper,
            IAdminService adminService,
            UserManager<IdentityUser> userManager, 
            ICommentService commentService)
        {
            this._recipesService = recipesService;
            this._mapper = mapper;
            this._adminService = adminService;
            this._userManager = userManager;
            this._commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> All(string id)
        {
            var recipe = await this._recipesService.ReadAsync(Guid.Parse(id));           
            var allComments = await this._commentService.FindAllAsync();
            var commentsForRecipe = allComments.Where(x => x.RecipeId == id).ToList();
            var recipeCommentsViewModel = _mapper.Map<List<CommentViewModel>>(allComments);
            this.ViewData["RecipeId"] = id;
            foreach (var commentModel in recipeCommentsViewModel)
            {
                commentModel.RecipeName = recipe.Name;
                commentModel.RecipeId = recipe.Id;
                commentModel.ImageUrl = recipe.ImageUrl;
                
            }
            foreach (var comments in recipeCommentsViewModel)
            {
                foreach (var comment in commentsForRecipe)
                {
                    var user = await _userManager.FindByIdAsync(comment.ApplicationUserId);
                    var userEmail = user.Email;
                    comments.SenderEmail = userEmail;
                }
            }
            if(recipeCommentsViewModel.Count == 0)
            {
                recipeCommentsViewModel.Add(new CommentViewModel
                {
                    RecipeId = id,
                    RecipeName = recipe.Name,
                    ImageUrl = recipe.ImageUrl
                });
            }
            return View(recipeCommentsViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Add(CommentSendModel model)
        {
            var allUsers = await this._adminService.GetAllUsersAsync();
            var usersViewModels = this._mapper.Map<IList<UserDetailsViewModel>>(allUsers);
            var isUserRegistered = allUsers.FirstOrDefault(x => x.Email == model.SenderEmail);           
            var user = await this._userManager.GetUserAsync(HttpContext.User);
            var currentUserEmail = user.Email;                      
            var userID = user.Id;
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please insert valid form data!" });
            }
            if (currentUserEmail != model.SenderEmail)
            {
                return Json(new { success = false, message = "You cannot add comment from other users emails" });
            }
            if (isUserRegistered == null)
            {
                return Json(new { success = false, message = "There isn't registered user with this Email address!" });
            }
            var comment = new CommentModel
            {
                Description = model.CommentMessage,
                RecipeId = model.RecipeId,
                ApplicationUserId = userID,
                CreatedOn = DateTime.Now,
                
            };
            await this._commentService.CreateAsync(comment);
            return Json(new { success = true, message = "You have added successfully a comment",commentModel = model });
        }
    }
}
