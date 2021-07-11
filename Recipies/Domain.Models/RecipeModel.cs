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

        public int PortionsSize { get; set; }


        public int TimeToPrepare { get; set; }

        public string PreparationDescription { get; set; }

        public DateTime CreatedOn { get; set; }

        public int NumberOfLikes { get; set; }

        public int NumberOfComments { get; set; }

        public int TotalCalories { get; set; }

        public string ApplicationUserId { get; set; }

        public string CategoryId { get; set; }

        public string DishId { get; set; }
    }
}
