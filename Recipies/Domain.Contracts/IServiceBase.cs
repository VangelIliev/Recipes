using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Contracts
{
    public interface IServiceBase<T>
    {
        Task<List<T>> FindAllAsync();

        Task<Guid> CreateAsync(T entity);

        Task<T> ReadAsync(Guid id);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteAsync(Guid id);
    }
}
