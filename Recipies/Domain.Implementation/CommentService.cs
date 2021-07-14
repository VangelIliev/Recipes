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
    public class CommentService : ServiceBase, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository, IMapper automapper) : base(automapper)
        {
            this._commentRepository = commentRepository;
        }
        public async Task<Guid> CreateAsync(CommentModel entity)
        {
            var dbEntity = this._autoMapper.Map<Comment>(entity);
            var result = await this._commentRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(CommentModel entity)
        {
            var dbEntity = _autoMapper.Map<Comment>(entity);
            await _commentRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _commentRepository.DeleteAsync(id);
        }

        public async Task<List<CommentModel>> FindAllAsync()
        {
            var dbEntities = await this._commentRepository.FindAllAsync();
            var result = _autoMapper.Map<List<CommentModel>>(dbEntities);
            return result;
        }
        //Remove     

        public async Task<CommentModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._commentRepository.ReadAsync(id);
            var result = _autoMapper.Map<CommentModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(CommentModel entity)
        {
            var dbEntity = this._autoMapper.Map<Comment>(entity);
            await this._commentRepository.UpdateAsync(dbEntity);
        }
    }
}
