using Recipes.Data.Models.Interfaces;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Models.Entities
{
    public class RecipeDislikes : IBaseEntity
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        //add user

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
