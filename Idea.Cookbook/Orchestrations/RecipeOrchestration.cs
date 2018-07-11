using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Idea.Cookbook.Extensions;
using Idea.Cookbook.Models;
using Idea.Cookbook.Services;

namespace Idea.Cookbook.Orchestrations
{
    public class RecipeOrchestration
    {
        private readonly RecipeService _recipeService;

        private readonly IMapper _mapper;

        public RecipeOrchestration(
            IMapper mapper, 
            RecipeService recipeService)
        {
            _recipeService = recipeService;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<Recipe>> GetRecipesAsync() =>
            _mapper.Map<IReadOnlyCollection<Recipe>>(await _recipeService.GetRecipesAsync().ConfigureAwait(false));

        public async Task<Recipe> GetRecipeAsync(Guid id) =>
            _mapper.Map<Recipe>(await _recipeService.GetRecipeAsync(id).ConfigureAwait(false));

        public async Task<IReadOnlyCollection<Ingredient>> GetRecipeIngredientsAsync(Guid id)
        {
            var recipe = await _recipeService.GetRecipeAsync(id).ConfigureAwait(false);
            return _mapper.Map<IReadOnlyCollection<Ingredient>>(recipe.Ingredients);
        }

        public Task<Recipe> CreateRecipeAsync(RecipeCreate ingredient) =>
            _mapper.Bridge<Recipe, Entities.Recipe>(() => 
                _recipeService.CreateRecipeAsync(_mapper.Map<Entities.Recipe>(ingredient)));

        public Task<Recipe> UpdateRecipeAsync(Guid id, RecipeUpdate unit) =>
            _mapper.Bridge<Recipe, Entities.Recipe>(
                () => _recipeService.UpdateRecipeAsync(id, _mapper.Map<Entities.Recipe>(unit)));

        public Task RemoveRecipeAsync(Guid id) => _recipeService.RemoveRecipeAsync(id);
    }
}