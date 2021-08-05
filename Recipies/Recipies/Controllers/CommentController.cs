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
            var commentViewModel = _mapper.Map<CommentViewModel>(recipe);
            commentViewModel.RecipeName = recipe.Name;
            commentViewModel.RecipeId = recipe.Id;
            return View(commentViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Add(CommentViewModel model)
        {
            var allUsers = await this._adminService.GetAllUsersAsync();
            var usersViewModels = this._mapper.Map<IList<UserDetailsViewModel>>(allUsers);
            var isUserRegistered = allUsers.FirstOrDefault(x => x.Email == model.SenderEmail);
            var user = await this._userManager.GetUserAsync(HttpContext.User);
            model.CommentCreation = DateTime.Now.ToShortDateString();
            var userID = user.Id;
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please insert valid form data!" });
            }
            if (isUserRegistered == null)
            {
                return Json(new { success = false, message = "There isn't registered user with this Email address!" });
            }
            var comment = new CommentModel
            {
                Description = model.CommentMessage,
                RecipeId = model.RecipeId,
                ApplicationUserId = userID
            };
            await this._commentService.CreateAsync(comment);
            return Json(new { success = true, message = "You have added successfully a comment",commentModel = model });
        }
    }
}
