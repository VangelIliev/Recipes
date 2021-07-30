using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Models.AdminModels;
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
        public AdminController(
                ICategoryService categoryService,
                IMapper mapper,
                ILogger logger,
                IAdminService adminService)
        {
            this._categoryService = categoryService;
            this._mapper = mapper;
            this._logger = (ILogger<AdminController>)logger;
            this._adminService = adminService;
        }
        [HttpGet]
        
        public IActionResult AddCategory()
        {
            AddCategoryViewModel model = new AddCategoryViewModel();
            return View("AddCategory",model);
        }

        [HttpPost]
        
        public async Task<ActionResult> AddCategory(AddCategoryViewModel model)
        {
            var categoryModel = _mapper.Map<CategoryModel>(model);
            await _categoryService.CreateAsync(categoryModel);
            return Redirect("/Recipes/All");
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
    }
}
