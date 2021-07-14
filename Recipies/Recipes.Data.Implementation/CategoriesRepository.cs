using Microsoft.EntityFrameworkCore;
using Recipes.Data.Contracts;
using Recipies.Data.Models.DbContext;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Implementation
{
    public class CategoriesRepository : RepositoryBase<Category>, ICategoriesRepository
    {
        public CategoriesRepository(RecipiesDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<List<string>> FindAllCategoryNames()
        {
            var a = await _dbContext.Categories.Distinct().ToListAsync();
            var categoryNames = a.Select(x => x.Name).ToList();
            return categoryNames;
        }
    }
}
