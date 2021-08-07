using Recipes.Data.Models.Entities;
using Recipes.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipies.Data.Models.DataConstants;
namespace Recipies.Data.Models.Entities
{
    public class Recipe : IBaseEntity
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int PortionsSize { get; set; }
        [Required]
        public int TimeToPrepare { get; set; }
        [Required]
        [MinLength(RecipeDescriptionMinLength)]
        [MaxLength(RecipeDescriptionMaxLength)]
        public string PreparationDescription { get; set; }
        //User who created It
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public int NumberOfComments { get; set; }
        public int TotalCalories { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }              
        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<RecipeProducts> Ingredients { get; set; } = new List<RecipeProducts>();

        public IEnumerable<Image>  Images { get; set; } = new List<Image>();
    }
}
