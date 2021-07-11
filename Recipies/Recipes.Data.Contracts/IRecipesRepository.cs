using Recipes.Domain.Models;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Contracts
{
    public interface IRecipesRepository : IRepositoryBase<Recipe>
    {
        Task<IEnumerable<Recipe>> FindAllRecipes();
    }
}
