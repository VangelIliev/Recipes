using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class LikeModel
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }       

        //add user

        public string ApplicationUserId { get; set; }
    }
}
