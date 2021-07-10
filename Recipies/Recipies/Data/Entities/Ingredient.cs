using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Data.Entities
{
    public class Ingredient
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
