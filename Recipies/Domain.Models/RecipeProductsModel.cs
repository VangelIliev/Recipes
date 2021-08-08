using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class RecipeProductsModel
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }

        public RecipeModel Recipe { get; set; }

        public Guid ProductId { get; set; }

        public ProductModel Product { get; set; }

        public string Quantity { get; set; }
    }
}
