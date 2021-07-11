using Microsoft.EntityFrameworkCore;
using Recipes.Data.Contracts;
using Recipes.Domain.Models;
using Recipies.Data.Models.DbContext;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Implementation
{
    public class RecipesRepository : RepositoryBase<Recipe>,IRecipesRepository
    {
        public RecipesRepository(RecipiesDbContext dbContext) : base(dbContext)
        {

        }

       
        public async Task<IEnumerable<Recipe>> FindAllRecipes()
        {
            var a = await this._dbContext.Recipes.ToListAsync();
            return a;
        }
    }
}
