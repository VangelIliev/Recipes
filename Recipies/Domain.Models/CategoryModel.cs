using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class CategoryModel
    {       
        public Guid Id { get; set; }
       
        public string Name { get; set; }

        public IEnumerable<RecipeModel> Recipes { get; set; } = new List<RecipeModel>();
    }
}
