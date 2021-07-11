using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipies.Data.Models.DataConstants;
namespace Recipies.Data.Models.Entities
{
    public class Dish
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MinLength(DishMinLength)]
        [MaxLength(DishMaxLength)]
        public string Name { get; set; }


        public IEnumerable<Recipe> Recipes = new List<Recipe>();
    }
}
