using AutoMapper;
using Recipes.Domain.Models;
using Recipies.Data.Models.Entities;
using Recipies.Models.RecipesModels;
using System.Collections.Generic;

namespace Recipies.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Recipe, RecipeModel>();           
            CreateMap<RecipeModel, Recipe>();
            CreateMap<RecipeModel, RecipeViewModel>();
            CreateMap<RecipeViewModel, RecipeModel>();
        }
    }
}
