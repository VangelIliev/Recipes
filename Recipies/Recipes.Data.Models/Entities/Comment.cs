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
    public class Comment : IBaseEntity
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(CommentDescriptionMinLength)]
        [MaxLength(CommentDescriptionMaxLength)]
        public string Description { get; set; }

        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        //Add user

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
