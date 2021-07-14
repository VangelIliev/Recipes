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
        public string ImageUrl { get; set; }
        
        public int PortionsSize { get; set; }
        
        public int TimeToPrepare { get; set; }
        [MaxLength(RecipeDescriptionMaxLength)]
        [MinLength(RecipeDescriptionMinLength)]
        public string PreparationDescription { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public int NumberOfLikes { get; set; }
        
        public int NumberOfComments { get; set; }

        public int TotalCalories { get; set; }

        public string ApplicationUserId { get; set; }
      
        public string CategoryId { get; set; }
              
        public string Category { get; set; }

        public IEnumerable<string> Categories { get; set; }

    }
}
