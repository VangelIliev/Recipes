using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Recipes.Domain.Models.ViewModelConstants;
namespace Recipies.Models.RecipesModels
{
    public class RecipeViewModel
    {

        public string Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int PortionsSize { get; set; }
        [Required]
        public int TimeToPrepare { get; set; }
        [Required]
        [MaxLength(RecipeDescriptionMaxLength)]
        [MinLength(10)]
        public string PreparationDescription { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public int NumberOfLikes { get; set; }
        
        public int NumberOfComments { get; set; }

        public int TotalCalories { get; set; }

        public string ApplicationUserId { get; set; }

        public string CreatedBy { get; set; }

        public string CategoryId { get; set; }
              
        public string Category { get; set; }

        public Dictionary<string,string> Categories { get; set; }
        

    }
}
