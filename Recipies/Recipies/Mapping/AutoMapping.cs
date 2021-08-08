using AutoMapper;
using Recipes.Data.Models.Entities;
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
            CreateMap<Microsoft.AspNetCore.Identity.IdentityUser, Recipes.Domain.Models.UserDetailsResponse>();

            CreateMap<Recipes.Domain.Models.UserDetailsResponse, Recipies.Models.AdminModels.UserDetailsViewModel>();
            CreateMap<Recipies.Models.AdminModels.UserDetailsViewModel, Recipes.Domain.Models.UserDetailsResponse>();

            CreateMap<Recipes.Domain.Models.UserDetailsResponse, Recipies.Models.AdminModels.EditUserViewModel>();

            CreateMap<Recipes.Domain.Models.EditUserRequest, Recipies.Models.AdminModels.EditUserViewModel>();
            CreateMap<Recipies.Models.AdminModels.EditUserViewModel, Recipes.Domain.Models.EditUserRequest>();

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
            CreateMap<CategoryModel, CategoryViewModel>();
            CreateMap<CategoryViewModel, CategoryModel>();

            CreateMap<RecipeModel, CommentModel>();
            CreateMap<CommentModel, RecipeModel>();
            CreateMap<RecipeModel, CommentViewModel>();
            CreateMap<CommentViewModel, RecipeModel>();
            CreateMap<CommentModel, CommentViewModel>();
            CreateMap<CommentViewModel, CommentModel>();

            CreateMap<RecipeDislikesModel, RecipeDislikes>();
            CreateMap<RecipeDislikes, RecipeDislikesModel>();

            CreateMap<RecipeProducts, RecipeProductsModel>();
            CreateMap<RecipeProductsModel, RecipeProducts>();
        }
    }
}
