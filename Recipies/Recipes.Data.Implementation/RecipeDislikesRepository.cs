using Recipes.Data.Contracts;
using Recipes.Data.Models.Entities;
using Recipies.Data.Models.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Implementation
{
    public class RecipeDislikesRepository : RepositoryBase<RecipeDislikes>, IRecipeDislikesRepository
    {
        public RecipeDislikesRepository(RecipiesDbContext dbContext) : base(dbContext)
        {

        }
    }
}
