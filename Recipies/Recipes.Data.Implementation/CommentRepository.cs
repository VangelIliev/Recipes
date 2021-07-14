using Recipes.Data.Contracts;
using Recipies.Data.Models.DbContext;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Implementation
{
     public class CommentRepository : RepositoryBase<Comment>,ICommentRepository
    {
        public CommentRepository(RecipiesDbContext dbContext) : base(dbContext)
        {

        }
    }
}
