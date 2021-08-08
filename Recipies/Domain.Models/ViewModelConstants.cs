using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Models
{
    public class ViewModelConstants
    {
        public const int ProductNameMinLength = 3;

        public const int ProductNameMaxLength = 15;

        public const int RecipeDescriptionMaxLength = 1000;

        public const int RecipeDescriptionMinLength = 30;

        public const int CategoryMinLength = 5;

        public const int CategoryMaxLength = 15;

        public const int CommentDescriptionMinLength = 10;

        public const int CommentDescriptionMaxLength = 40;

        public const int DishMinLength = 5;

        public const int DishMaxLength = 20;
    }
}
