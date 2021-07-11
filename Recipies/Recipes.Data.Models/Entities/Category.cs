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
    public class Category : IBaseEntity
    {
        [Required]
        [Key]
        public Guid Id { get; set; } 

        [Required]
        [MinLength(CategoryMinLength)]
        [MaxLength(CategoryMaxLength)]
        public string Name { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
