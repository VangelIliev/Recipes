using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Contracts
{
    public interface IRolesRepository
    {
        Task<bool> IsRoleAssignedAsync(string roleId);
    }
}
