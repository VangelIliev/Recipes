using Microsoft.EntityFrameworkCore;
using Recipes.Data.Contracts;
using Recipies.Data.Models.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Implementation
{
    public class RolesRepository : IRolesRepository
    {
        private readonly RecipiesDbContext _dbContext;

        public RolesRepository(RecipiesDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> IsRoleAssignedAsync(string roleId)
        {
            return await this._dbContext.UserRoles.AnyAsync(x => x.RoleId == roleId);
        }
    }
}
