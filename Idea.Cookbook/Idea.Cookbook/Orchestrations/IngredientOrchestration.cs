using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Idea.Cookbook.Extensions;
using Idea.Cookbook.Interfaces.Services;
using Idea.Cookbook.Models;

namespace Idea.Cookbook.Orchestrations
{
    public class IngredientOrchestration
    {
        private readonly IIngredientService _service;
        
        private readonly IMapper _mapper;

        public IngredientOrchestration(IMapper mapper, IIngredientService service)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<Ingredient>> GetIngredientsAsync() =>
            _mapper.Map<IReadOnlyCollection<Ingredient>>(await _service.GetIngredientsAsync().ConfigureAwait(false));

        public Task<Ingredient> CreateIngredientAsync(IngredientCreate ingredient) =>
            _mapper.Bridge<Ingredient, Entities.Ingredient>(
                () => _service.CreateIngredientAsync(_mapper.Map<Entities.Ingredient>(ingredient)));

        public Task<Ingredient> UpdateIngredientAsync(Guid id, IngredientUpdate ingredient) =>
            _mapper.Bridge<Ingredient, Entities.Ingredient>(
                () => _service.UpdateIngredientAsync(id, _mapper.Map<Entities.Ingredient>(ingredient)));

        public Task RemoveIngredientAsync(Guid id) => _service.RemoveIngredientAsync(id);
    }
}