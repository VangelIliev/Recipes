using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Recipies.Data.DataConstants;
namespace Recipies.Data.Entities
{
    public class Comment
    {
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MinLength(CommentDescriptionMinLength)]
        [MaxLength(CommentDescriptionMaxLength)]
        public string Description { get; set; }

        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        //Add user

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
