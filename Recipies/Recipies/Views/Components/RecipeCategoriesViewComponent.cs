using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Contracts;
using Recipies.Models.RecipesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipies.Views.Components
{
    public class RecipeCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public RecipeCategoriesViewComponent(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            this._mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.FindAllAsync();
            var categoriesViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            return View(categoriesViewModel);
        }
    }
}
