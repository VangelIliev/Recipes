using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class ProductModel
    {
        
        public Guid Id { get; set; }
       
        public string Name { get; set; }
        
        public decimal Weight { get; set; }
        
        public decimal Calories { get; set; }

        public Guid RecipeId { get; set; }
       
    }
}
