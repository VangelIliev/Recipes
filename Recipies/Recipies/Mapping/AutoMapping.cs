using AutoMapper;
using Recipies.Data.Entities;
using Recipies.Models.RecipesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Recipe, RecipeViewModel>();
            CreateMap<RecipeViewModel, Recipe>();
        }
    }
}
