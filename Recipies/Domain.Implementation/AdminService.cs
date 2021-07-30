using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Recipes.Data.Contracts;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Implementation
{
    public class AdminService : ServiceBase,IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRolesRepository _rolesRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRolesRepository rolesRepository,
            ILogger<AdminService> logger,
            IMapper autoMapper) : base(autoMapper)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._rolesRepository = rolesRepository;
            this._logger = logger;
        }

        #region Users
        public async Task<IList<UserDetailsResponse>> GetAllUsersAsync()
        {
            var response = new List<UserDetailsResponse>();

            var identityUsers = this._userManager.Users;
            foreach (var user in identityUsers)
            {
                var resultUserOject = this._autoMapper.Map<UserDetailsResponse>(user);
                var allRolesForUser = await this._userManager.GetRolesAsync(user);
                if (allRolesForUser != null) resultUserOject.Role = allRolesForUser.FirstOrDefault();
                response.Add(resultUserOject);
            }

            return response;
        }

        public async Task<UserDetailsResponse> GetUserByIdAsync(string userId)
        {
            var identityUser = await this._userManager.FindByIdAsync(userId);
            var response = this._autoMapper.Map<UserDetailsResponse>(identityUser);
            var allRolesForUser = await this._userManager.GetRolesAsync(identityUser);
            if (allRolesForUser != null) response.Role = allRolesForUser.FirstOrDefault();
            return response;
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            var response = new CreateUserResponse();

            try
            {
                var user = new IdentityUser
                {
                    UserName = createUserRequest.Email,
                    Email = createUserRequest.Email
                };

                var createResult = await this._userManager.CreateAsync(user, createUserRequest.Password);
                if (createResult.Succeeded)
                {
                    var addToRoleResult = await this._userManager.AddToRoleAsync(user, createUserRequest.Role);
                    if (addToRoleResult.Succeeded)
                    {
                        response.Success = true;
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                        {
                            response.Errors.Add(error.Description);
                        }
                    }

                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        response.Errors.Add(error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        public async Task<bool> EditUserAsync(string userId, EditUserRequest editUserRequest)
        {
            try
            {
                var identityUser = await this._userManager.FindByIdAsync(userId);

                if (!string.IsNullOrEmpty(editUserRequest.Role))
                {
                    var removeRoleResult = await this._userManager.RemoveFromRoleAsync(identityUser, editUserRequest.Role);
                    if (!removeRoleResult.Succeeded)
                    {
                        this.LogIdentityErrors(removeRoleResult.Errors, nameof(EditUserAsync));
                        return false;
                    }
                }

                var addRoleResult = await this._userManager.AddToRoleAsync(identityUser, editUserRequest.NewRole);
                if (!addRoleResult.Succeeded)
                {
                    this.LogIdentityErrors(addRoleResult.Errors, nameof(EditUserAsync));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var identityUser = await this._userManager.FindByIdAsync(userId);
            var deleteUserResut = await this._userManager.DeleteAsync(identityUser);
            if (!deleteUserResut.Succeeded)
            {
                this.LogIdentityErrors(deleteUserResut.Errors, nameof(DeleteUserAsync));
                return false;
            }

            return true;
        }

        #endregion

        #region Roles
        public IList<IdentityRole> GetAllRoles()
        {
            return this._roleManager.Roles.ToList();
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await this._roleManager.FindByIdAsync(roleId);
        }

        public async Task<bool> CreateNewRoleAsync(IdentityRole indentityRole)
        {
            var isRoleExist = await this._roleManager.RoleExistsAsync(indentityRole.Name);
            if (isRoleExist)
            {
                return true;
            }

            var createNewRoleResult = await this._roleManager.CreateAsync(indentityRole);
            if (!createNewRoleResult.Succeeded)
            {
                LogIdentityErrors(createNewRoleResult.Errors, nameof(CreateNewRoleAsync));
                return false;
            }

            return true;
        }

        public async Task<bool> EditRoleAsync(string roleId, string roleNewName)
        {
            try
            {
                var identityRole = await this._roleManager.FindByIdAsync(roleId);
                identityRole.Name = roleNewName;

                var updatedRoleResult = await this._roleManager.UpdateAsync(identityRole);
                if (!updatedRoleResult.Succeeded)
                {
                    LogIdentityErrors(updatedRoleResult.Errors, nameof(EditRoleAsync));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            try
            {
                if (await this._rolesRepository.IsRoleAssignedAsync(roleId))
                {
                    return false;
                }

                var identityRole = await this._roleManager.FindByIdAsync(roleId);
                var deleteRoleResult = await this._roleManager.DeleteAsync(identityRole);
                if (!deleteRoleResult.Succeeded)
                {
                    LogIdentityErrors(deleteRoleResult.Errors, nameof(DeleteRoleAsync));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return false;
            }
        }

        #endregion

        private void LogIdentityErrors(IEnumerable<IdentityError> errors, string methodName)
        {
            foreach (var error in errors)
            {
                this._logger.LogError($"Error On {methodName}. Error Code {error.Code}. Error Description {error.Description}");
            }
        }
    }
}
