using Recipes.Data.Models.Interfaces;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Models.Entities
{
    public class Image : IBaseEntity
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string Extension { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
