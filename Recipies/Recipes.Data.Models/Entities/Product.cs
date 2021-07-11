﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipies.Data.Models.DataConstants;
namespace Recipies.Data.Models.Entities
{
    public class Product
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MinLength(ProductNameMinLength)]
        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Weight { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Calories { get; set; }

        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public IEnumerable<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}
