using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipies.Data.Models.Entities
{
    public class Like
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        //add user

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
