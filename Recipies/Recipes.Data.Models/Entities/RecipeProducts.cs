using Recipes.Data.Models.Interfaces;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Models.Entities
{
    public class RecipeProducts : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public string Quantity { get; set; }
    }
}
