using Autofac.Extras.Moq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Recipes.Data.Contracts;
using Recipes.Domain.Implementation;
using RecipesTests.Models;
using System;
using Xunit;

namespace RecipesTests
{
    public class AdminServicesTests
    {
        public static Mock<UserManager<IdentityUser>> _usermanager = new Mock<UserManager<IdentityUser>>();
        public static Mock<RoleManager<IdentityRole>> _roleManager = new Mock<RoleManager<IdentityRole>>();
        public static Mock<IRolesRepository> _rolesRepository = new Mock<IRolesRepository>();
        public static Mock<ILogger<AdminService>> _logger = new Mock<ILogger<AdminService>>();
        public static Mock<IMapper> _mapper = new Mock<IMapper>();
        
        [Fact]
        public void TestCreateUserRoleNotExisting()
        {
            
            var adminService = new AdminService(
                _usermanager.Object, 
                _roleManager.Object, 
                _rolesRepository.Object, 
                _logger.Object, 
                _mapper.Object);

            var role = new IdentityRole();
            role.Name = "Administrator";
            var users = adminService.CreateNewRoleAsync(role).Result;
            Assert.True(users);
        }
    }
}
