using AutoMapper;
using Recipes.Data.Contracts;
using Recipes.Domain.Contracts;
using Recipes.Domain.Models;
using Recipies.Data.Models.DbContext;
using Recipies.Data.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Implementation
{
    public class RecipesService : ServiceBase, IRecipesService
    {
        private readonly IRecipesRepository _recipesRepository;
        public RecipesService(IRecipesRepository recipeRepository, IMapper automapper) : base(automapper)
        {
            this._recipesRepository = recipeRepository;
        }
        public async Task<Guid> CreateAsync(RecipeModel entity)
        {
            var dbEntity = this._autoMapper.Map<Recipe>(entity);
            var result = await this._recipesRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(RecipeModel entity)
        {
            var dbEntity = _autoMapper.Map<Recipe>(entity);
            await _recipesRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _recipesRepository.DeleteAsync(id);
        }

        public async Task<List<RecipeModel>> FindAllAsync()
        {
            var dbEntities = await this._recipesRepository.FindAllAsync();
            var result = _autoMapper.Map<List<RecipeModel>>(dbEntities);
            return result;
        }
        //Remove
        public async Task<List<RecipeModel>> FindAllRecipesAsync()
        {
            var dbEntities = await this._recipesRepository.FindAllAsync();
            var result = _autoMapper.Map<List<RecipeModel>>(dbEntities);
            return result;
        }

        public async Task<RecipeModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._recipesRepository.ReadAsync(id);
            var result = _autoMapper.Map<RecipeModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(RecipeModel entity)
        {
            var dbEntity = this._autoMapper.Map<Recipe>(entity);
            await this._recipesRepository.UpdateAsync(dbEntity);
        }
    }
}
