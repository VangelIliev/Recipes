using AutoMapper;
using Recipes.Domain.Models;
using Recipies.Data.Models.Entities;
using Recipies.Models.AdminModels;
using Recipies.Models.CommentModels;
using Recipies.Models.RecipesModels;
using System.Collections.Generic;

namespace Recipies.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Recipe
            CreateMap<Recipe, RecipeModel>();           
            CreateMap<RecipeModel, Recipe>();
            CreateMap<RecipeModel, RecipeViewModel>();
            CreateMap<RecipeViewModel, RecipeModel>();

            CreateMap<Like, LikeModel>();
            CreateMap<LikeModel, Like>();

            CreateMap<Comment, CommentModel>();
            CreateMap<CommentModel, Comment>();

            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();

            CreateMap<AddCategoryViewModel, CategoryModel>();
            CreateMap<CategoryModel, AddCategoryViewModel>();
            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>();

            CreateMap<RecipeModel, CommentModel>();
            CreateMap<CommentModel, RecipeModel>();
            CreateMap<RecipeModel, CommentViewModel>();
            CreateMap<CommentViewModel, RecipeModel>();
        }
    }
}
