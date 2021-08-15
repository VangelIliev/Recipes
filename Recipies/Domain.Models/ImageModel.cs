
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class ImageModel
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }
        public string UserId { get; set; }        

        public string Extension { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FilePath { get; set; }

        public string ImageName { get; set; }
    }
}
