using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Recipies.Data.DataConstants;
namespace Recipies.Data.Entities
{
    public class Category
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MinLength(CategoryMinLength)]
        [MaxLength(CategoryMaxLength)]
        public string Name { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
