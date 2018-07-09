using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;

namespace Idea.Cookbook.Interfaces.Services
{
    public interface IIngredientService
    {
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);

        Task<Ingredient> UpdateIngredientAsync(Guid id, Ingredient unit);

        Task RemoveIngredientAsync(Guid id);

        Task<IReadOnlyCollection<Ingredient>> GetIngredientsAsync();

        Task<Ingredient> GetIngredientAsync(Guid id);
    }
}