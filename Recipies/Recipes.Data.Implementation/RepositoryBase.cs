using Microsoft.EntityFrameworkCore;
using Recipes.Data.Contracts;
using Recipes.Data.Models.Interfaces;
using Recipies.Data.Models.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Data.Implementation
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class,IBaseEntity
    {
        protected readonly RecipiesDbContext _dbContext;

        public RepositoryBase(RecipiesDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(T entity)
        {
            try
            {
                this._dbContext.Set<T>().Add(entity);
                await this._dbContext.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
            await this._dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await this._dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            this._dbContext.Set<T>().Remove(entity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> FindAllAsync()
        {
            var a = await this._dbContext.Set<T>().AsNoTracking().ToListAsync();
            return await this._dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> ReadAsync(Guid id)
        {
            return await this._dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            this._dbContext.Set<T>().Update(entity);
            await this._dbContext.SaveChangesAsync();
        }
    }
}
