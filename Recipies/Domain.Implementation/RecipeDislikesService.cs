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
    public class RecipeDislikesService : ServiceBase, IRecipeDislikesService
    {
        private readonly IRecipeDislikesRepository _recipeDislikesRepository;
        public RecipeDislikesService(IRecipeDislikesRepository recipeDislikesRepository, IMapper automapper) : base(automapper)
        {
            this._recipeDislikesRepository = recipeDislikesRepository;
        }
        public async Task<Guid> CreateAsync(RecipeDislikesModel entity)
        {
            var dbEntity = this._autoMapper.Map<RecipeDislikes>(entity);
            var result = await this._recipeDislikesRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(RecipeDislikesModel entity)
        {
            var dbEntity = _autoMapper.Map<RecipeDislikes>(entity);
            await _recipeDislikesRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _recipeDislikesRepository.DeleteAsync(id);
        }

        public async Task<List<RecipeDislikesModel>> FindAllAsync()
        {
            var dbEntities = await this._recipeDislikesRepository.FindAllAsync();
            var result = _autoMapper.Map<List<RecipeDislikesModel>>(dbEntities);
            return result;
        }
        //Remove     

        public async Task<RecipeDislikesModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._recipeDislikesRepository.ReadAsync(id);
            var result = _autoMapper.Map<RecipeDislikesModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(RecipeDislikesModel entity)
        {
            var dbEntity = this._autoMapper.Map<RecipeDislikes>(entity);
            await this._recipeDislikesRepository.UpdateAsync(dbEntity);
        }
    }
}
