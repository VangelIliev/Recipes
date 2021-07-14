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
    public class ProductRepository : RepositoryBase<Product>,IProductsRepository
    {
        public ProductRepository(RecipiesDbContext dbContext) : base(dbContext)
        {

        }
    }
}
