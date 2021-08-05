using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Models.CommentModels
{
    public class CommentViewModel
    {
        public CommentSendModel SendCommentModel { get; set; }
        public string RecipeId { get; set; }
        public string RecipeName { get; set; }

        public string ImageUrl { get; set; }

        public string LastCommentDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string CommentMessage { get; set; }
        [Required]
        [EmailAddress]
        public string SenderEmail { get; set; }

        public string CommentCreation { get; set; }
    }
}
