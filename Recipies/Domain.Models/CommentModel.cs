using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class CommentModel
    {
        
        public Guid Id { get; set; }
        
        public string Description { get; set; }

        public string RecipeId { get; set; }
        
        //Add user

        public string ApplicationUserId { get; set; }

        
    }
}
