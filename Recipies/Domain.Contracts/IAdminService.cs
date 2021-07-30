using Recipes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Recipes.Domain.Contracts
{
    public interface IAdminService
    {
        // users
        Task<IList<UserDetailsResponse>> GetAllUsersAsync();
        Task<UserDetailsResponse> GetUserByIdAsync(string userId);
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<bool> EditUserAsync(string userId, EditUserRequest editUserRequest);
        Task<bool> DeleteUserAsync(string userId);

        // roles
        IList<IdentityRole> GetAllRoles();
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<bool> CreateNewRoleAsync(IdentityRole indentityRole);
        Task<bool> EditRoleAsync(string roleId, string roleNewName);
        Task<bool> DeleteRoleAsync(string roleId);

    }
}
