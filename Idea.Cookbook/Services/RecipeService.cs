using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;
using Idea.Cookbook.Queries;
using Idea.NetCore.EntityFrameworkCore;
using Idea.Query.EntityFrameworkCore.Extensions;
using Idea.Repository;
using Idea.UnitOfWork;

namespace Idea.Cookbook.Services
{
    public class RecipeService
    {
        private readonly IUnitOfWorkFactory _factory;

        private readonly IRepository<Recipe, Guid> _recipeRepository;

        private readonly IRepository<Ingredient, Guid> _ingredientRepository;

        public RecipeService(
            IUnitOfWorkFactory factory,
            IRepository<Recipe, Guid> recipeRepository,
            IRepository<Ingredient, Guid> ingredientRepository)
        {
            _factory = factory;
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            using (var uow = _factory.Create())
            {
                recipe.Ingredients = await UpdateIngrediencesAsync(recipe).ConfigureAwait(false);
                await _recipeRepository.CreateAsync(recipe).ConfigureAwait(false);
                await uow.CommitAsync().ConfigureAwait(false);

                return recipe;
            }
        }

        public async Task<Recipe> UpdateRecipeAsync(Guid id, Recipe recipe)
        {
            using (var uow = _factory.Create())
            {
                var ingredients = await UpdateIngrediencesAsync(recipe).ConfigureAwait(false);
                var entity = await _recipeRepository.ComulativeUpdateAsync(id,
                        u => u.Name = recipe.Name,
                        u => u.Directions = recipe.Directions,
                        u => u.Ingredients = ingredients).ConfigureAwait(false);
                
                await uow.CommitAsync().ConfigureAwait(false);
                return entity;
            }
        }

        public Task RemoveRecipeAsync(Guid id) => _factory.With(_recipeRepository).DeleteAndCommitAsync(id);

        public Task<IReadOnlyCollection<Recipe>> GetRecipesAsync() => new ReadRecipes().ExecuteAsync(_factory);

        public Task<Recipe> GetRecipeAsync(Guid id) => new FetchRecipe(id).FetchAsync(_factory);

        private async Task<List<Ingredient>> UpdateIngrediencesAsync(Recipe recipe) =>
            (await Task.WhenAll(
                 recipe.Ingredients.Select(ingredient => 
                     _ingredientRepository.ComulativeUpdateAsync(ingredient.Id, m => m.RecipeId = recipe.Id)))
                 .ConfigureAwait(false)).ToList();
    }
}