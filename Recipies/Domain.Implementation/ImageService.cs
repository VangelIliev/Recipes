using AutoMapper;
using Recipes.Data.Implementation;
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
    public class ImageService : ServiceBase, IImageService
    {
        private readonly ImageRepository _imageRepository;
        public ImageService(ImageRepository imageRepository, IMapper automapper) : base(automapper)
        {
            this._imageRepository = imageRepository;
        }
        public async Task<Guid> CreateAsync(ImageModel entity)
        {
            var dbEntity = this._autoMapper.Map<Image>(entity);
            var result = await this._imageRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(ImageModel entity)
        {
            var dbEntity = _autoMapper.Map<Image>(entity);
            await _imageRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _imageRepository.DeleteAsync(id);
        }

        public async Task<List<ImageModel>> FindAllAsync()
        {
            var dbEntities = await this._imageRepository.FindAllAsync();
            var result = _autoMapper.Map<List<ImageModel>>(dbEntities);
            return result;
        }

        public  async Task<ImageModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._imageRepository.ReadAsync(id);
            var result = _autoMapper.Map<ImageModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(ImageModel entity)
        {
            var dbEntity = this._autoMapper.Map<Image>(entity);
            await this._imageRepository.UpdateAsync(dbEntity);
        }
    }
}
