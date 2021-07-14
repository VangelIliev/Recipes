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
    public class LikeService : ServiceBase,ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        public LikeService(ILikeRepository likeRepository, IMapper automapper) : base(automapper)
        {
            this._likeRepository = likeRepository;
        }
        public async Task<Guid> CreateAsync(LikeModel entity)
        {
            var dbEntity = this._autoMapper.Map<Like>(entity);
            var result = await this._likeRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(LikeModel entity)
        {
            var dbEntity = _autoMapper.Map<Like>(entity);
            await _likeRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _likeRepository.DeleteAsync(id);
        }

        public async Task<List<LikeModel>> FindAllAsync()
        {
            var dbEntities = await this._likeRepository.FindAllAsync();
            var result = _autoMapper.Map<List<LikeModel>>(dbEntities);
            return result;
        }
        //Remove     

        public async Task<LikeModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._likeRepository.ReadAsync(id);
            var result = _autoMapper.Map<LikeModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(LikeModel entity)
        {
            var dbEntity = this._autoMapper.Map<Like>(entity);
            await this._likeRepository.UpdateAsync(dbEntity);
        }
    }
}
