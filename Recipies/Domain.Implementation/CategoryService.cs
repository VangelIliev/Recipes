using AutoMapper;
using Recipes.Data.Contracts;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Implementation
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        private readonly ICategoriesRepository _categoriesRepository;
        public CategoryService(ICategoriesRepository categoriesRepository, IMapper automapper) : base(automapper)
        {
            this._categoriesRepository = categoriesRepository;
        }
        public async Task<Guid> CreateAsync(CategoryModel entity)
        {
            var dbEntity = this._autoMapper.Map<Category>(entity);
            var result = await this._categoriesRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(CategoryModel entity)
        {
            var dbEntity = _autoMapper.Map<Category>(entity);
            await _categoriesRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoriesRepository.DeleteAsync(id);
        }

        public async Task<List<CategoryModel>> FindAllAsync()
        {
            var dbEntities = await this._categoriesRepository.FindAllAsync();
            var result = _autoMapper.Map<List<CategoryModel>>(dbEntities);
            return result;
        }

        public Task<List<string>> FindAllCategoryNames()
        {
            var names = _categoriesRepository.FindAllCategoryNames();
            return names;
        }

        public async Task<CategoryModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._categoriesRepository.ReadAsync(id);
            var result = _autoMapper.Map<CategoryModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(CategoryModel entity)
        {
            var dbEntity = this._autoMapper.Map<Category>(entity);
            await this._categoriesRepository.UpdateAsync(dbEntity);
        }
    }
}
