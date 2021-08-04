using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
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

        public CommentController(
            IRecipesService recipesService, 
            IMapper mapper, 
            IAdminService adminService)
        {
            this._recipesService = recipesService;
            this._mapper = mapper;
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> All(string id)
        {
            var recipe = await this._recipesService.ReadAsync(Guid.Parse(id));
            var commentViewModel = _mapper.Map<CommentViewModel>(recipe);
            commentViewModel.RecipeName = recipe.Name;
            ;
            return View(commentViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Add(CommentViewModel model)
        {
            var allUsers = await this._adminService.GetAllUsersAsync();
            var usersViewModels = this._mapper.Map<IList<UserDetailsViewModel>>(allUsers);
            var isUserRegistered = allUsers.FirstOrDefault(x => x.Email == model.SenderEmail);
            if (isUserRegistered == null)
            {
                return Json(new { success = false, message = "There isn't registered user with this Email address!" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please insert valid form data!" });
            }
            return Json(new { success = true, message = "You have added successfully a comment"});
        }
    }
}
