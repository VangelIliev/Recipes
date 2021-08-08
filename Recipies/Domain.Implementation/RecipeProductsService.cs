using AutoMapper;
using Recipes.Data.Contracts;
using Recipes.Data.Models.Entities;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Implementation
{
    public class RecipeProductsService : ServiceBase, IRecipeProductsService
    {
        private readonly IRecipeProductsRepository _recipeProductsRepository;
        public RecipeProductsService(IRecipeProductsRepository recipeProductsRepository, IMapper automapper) : base(automapper)
        {
            this._recipeProductsRepository = recipeProductsRepository;
        }
        public async Task<Guid> CreateAsync(RecipeProductsModel entity)
        {
            var dbEntity = this._autoMapper.Map<RecipeProducts>(entity);
            var result = await this._recipeProductsRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(RecipeProductsModel entity)
        {
            var dbEntity = _autoMapper.Map<RecipeProducts>(entity);
            await _recipeProductsRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _recipeProductsRepository.DeleteAsync(id);
        }

        public async Task<List<RecipeProductsModel>> FindAllAsync()
        {
            var dbEntities = await this._recipeProductsRepository.FindAllAsync();
            var result = _autoMapper.Map<List<RecipeProductsModel>>(dbEntities);
            return result;
        }

        public async Task<RecipeProductsModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._recipeProductsRepository.ReadAsync(id);
            var result = _autoMapper.Map<RecipeProductsModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(RecipeProductsModel entity)
        {
            var dbEntity = this._autoMapper.Map<RecipeProducts>(entity);
            await this._recipeProductsRepository.UpdateAsync(dbEntity);
        }
    }
}
