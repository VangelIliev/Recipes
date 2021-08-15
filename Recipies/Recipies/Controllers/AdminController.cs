using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipies.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILikeService _likeService;
        private readonly IRecipesService _recipesService;
        public AdminController(
                ICategoryService categoryService,
                IMapper mapper,
                ILogger<AdminController> logger,
                IAdminService adminService,
                SignInManager<IdentityUser> signInManager,
                UserManager<IdentityUser> userManager,
                ILikeService likeService,
                IRecipesService recipesService
                )
        {
            this._categoryService = categoryService;
            this._mapper = mapper;
            this._logger = logger;
            this._adminService = adminService;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._likeService = likeService;
            this._recipesService = recipesService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogInUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                           model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("All","Recipes");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("All", "Recipes");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> LogOut()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("All", "Recipes");
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.Roles = this._adminService.GetAllRoles();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            ViewBag.Roles = this._adminService.GetAllRoles();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var createUserRequest = new CreateUserRequest()
            {
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };

            var createUserResult = await this._adminService.CreateUserAsync(createUserRequest);
            if (!createUserResult.Success)
            {
                if (createUserResult.Errors != null)
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                        _logger.LogError(error);
                    }
                }

                return View();
            }

            return RedirectToAction("AllUsers");
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var allUsers = await this._adminService.GetAllUsersAsync();
            var usersViewModels = this._mapper.Map<IList<UserDetailsViewModel>>(allUsers);
            return View(usersViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsUser(string userId)
        {
            var user = await this._adminService.GetUserByIdAsync(userId);
            var userViewModel = this._mapper.Map<UserDetailsViewModel>(user);
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await this._adminService.GetUserByIdAsync(userId);
            var userViewModel = this._mapper.Map<EditUserViewModel>(user);
            ViewBag.Roles = this._adminService.GetAllRoles();
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var editUserRequest = this._mapper.Map<EditUserRequest>(model);
            var success = await this._adminService.EditUserAsync(model.Id, editUserRequest);
            return RedirectToAction("AllUsers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(UserDetailsViewModel model)
        {
            var deleteResult = await this._adminService.DeleteUserAsync(model.Id);
            return RedirectToAction("AllUsers");
        }

        [HttpGet]
        public IActionResult AllRoles()
        {
            var roles = this._adminService.GetAllRoles();
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            await this._adminService.CreateNewRoleAsync(role);
            return RedirectToAction("AllRoles");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsRole(string roleId)
        {
            var identityRole = await this._adminService.GetRoleByIdAsync(roleId);
            var role = new RoleDetailsViewModel { Id = roleId, Name = identityRole.Name };
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var identityRole = await this._adminService.GetRoleByIdAsync(roleId);
            return View(identityRole);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole role)
        {
            await this._adminService.EditRoleAsync(role.Id, role.Name);
            return RedirectToAction("AllRoles");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(IdentityRole role)
        {
            await this._adminService.DeleteRoleAsync(role.Id);
            return RedirectToAction("AllRoles");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            await this._adminService.DeleteRoleAsync(id);
            return RedirectToAction("AllRoles");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRecipe(string id)
        {
            var recipe = await this._recipesService.ReadAsync(Guid.Parse(id));
            await this._recipesService.DeleteAsync(recipe);
            return RedirectToAction("All","Recipes");
        }
    }
}
