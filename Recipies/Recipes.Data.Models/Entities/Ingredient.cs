using Recipes.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipies.Data.Models.Entities
{
    public class Ingredient : IBaseEntity
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
