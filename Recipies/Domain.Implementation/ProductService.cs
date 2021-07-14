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
    public class ProductService : ServiceBase,IProductService
    {
        private readonly IProductsRepository _productRepository;
        public ProductService(IProductsRepository productRepository, IMapper automapper) : base(automapper)
        {
            this._productRepository = productRepository;
        }
        public async Task<Guid> CreateAsync(ProductModel entity)
        {
            var dbEntity = this._autoMapper.Map<Product>(entity);
            var result = await this._productRepository.CreateAsync(dbEntity);
            return result;
        }

        public async Task DeleteAsync(ProductModel entity)
        {
            var dbEntity = _autoMapper.Map<Product>(entity);
            await _productRepository.DeleteAsync(dbEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<List<ProductModel>> FindAllAsync()
        {
            var dbEntities = await this._productRepository.FindAllAsync();
            var result = _autoMapper.Map<List<ProductModel>>(dbEntities);
            return result;
        }
        //Remove     

        public async Task<ProductModel> ReadAsync(Guid id)
        {
            var dbEntity = await this._productRepository.ReadAsync(id);
            var result = _autoMapper.Map<ProductModel>(dbEntity);
            return result;
        }

        public async Task UpdateAsync(ProductModel entity)
        {
            var dbEntity = this._autoMapper.Map<Product>(entity);
            await this._productRepository.UpdateAsync(dbEntity);
        }
    }
}
