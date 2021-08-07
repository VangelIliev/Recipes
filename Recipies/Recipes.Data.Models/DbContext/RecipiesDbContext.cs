using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipes.Data.Models.Entities;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipies.Data.Models.DbContext
{
    public class RecipiesDbContext : IdentityDbContext
    {
        public RecipiesDbContext()
        {

        }
        public RecipiesDbContext(DbContextOptions<RecipiesDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }              
        public DbSet<Like> Likes { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeProducts> RecipeProducts { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<RecipeDislikes> RecipeDislikes { get; set; }
    }
}
