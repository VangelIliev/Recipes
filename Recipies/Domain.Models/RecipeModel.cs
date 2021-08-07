using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class RecipeModel
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public int PortionsSize { get; set; }

        public int TimeToPrepare { get; set; }

        public string PreparationDescription { get; set; }

        public string CreatedOn { get; set; }

        public int NumberOfComments { get; set; }

        public int TotalCalories { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUserModel ApplicationUser { get; set; }
        public Guid CategoryId { get; set; }

        
    }
}
