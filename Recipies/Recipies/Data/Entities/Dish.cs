using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Recipies.Data.DataConstants;
namespace Recipies.Data.Entities
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
