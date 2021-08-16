
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Recipes.Data.Contracts;
using Recipes.Data.Implementation;
using Recipes.Domain.Contracts;
using Recipes.Domain.Implementation;
using Recipies.Data;
using Recipies.Data.Models.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RecipiesDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddTransient<RecipiesDbContext>();           
            services.AddIdentity<IdentityUser,IdentityRole>(options => 
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
             })
                .AddEntityFrameworkStores<RecipiesDbContext>();
            services.AddMvc();
            services.AddSession();
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(Mapping.AutoMapping));
            services.AddAntiforgery();            

            // REPOSITORIES
            services.AddTransient<ICategoriesRepository, CategoriesRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ILikeRepository, LikeRepository>();
            services.AddTransient<IProductsRepository, ProductRepository>();
            services.AddTransient<IRecipesRepository, RecipesRepository>();
            services.AddTransient<IRolesRepository, RolesRepository>();
            services.AddTransient<IRecipeDislikesRepository, RecipeDislikesRepository>();
            services.AddTransient<IRecipeProductsRepository, RecipeProductsRepository>();
            services.AddTransient<IImagesRepository, ImageRepository>();

            //SERVICES
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ILikeService, LikeService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IRecipesService, RecipesService>();
            services.AddTransient<IRecipeDislikesService, RecipeDislikesService>();
            services.AddTransient<IRecipeProductsService, RecipeProductsService>();
            services.AddTransient<IImageService, ImageService>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Errors/CustomError");
                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "MyArea", 
                    pattern: "{admin}/{controller=Home}/{action=LogIn}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Recipes}/{action=All}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
