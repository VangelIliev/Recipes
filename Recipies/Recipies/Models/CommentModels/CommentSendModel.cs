using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Models.CommentModels
{
    public class CommentSendModel
    {
        public string RecipeId { get; set; }

        public string CommentMessage { get; set; }

        public string SenderEmail { get; set; }

        public string CommentCreation { get; set; }
    }
}
