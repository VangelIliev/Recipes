using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Xunit;

namespace RecipesTests
{
    public class AdminServicesTests
    {
        private readonly AppDbContext appDbContext;

        public AdminServicesTests(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [Fact]
        public void TestGetAllUsers()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "Find_User_Database") // Give a Unique name to the DB.Options;
        }
    }
}
