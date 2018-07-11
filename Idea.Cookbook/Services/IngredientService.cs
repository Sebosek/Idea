using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;
using Idea.Cookbook.Interfaces.Services;
using Idea.Cookbook.Queries;
using Idea.NetCore.EntityFrameworkCore;
using Idea.Query.EntityFrameworkCore.Extensions;
using Idea.Repository;
using Idea.UnitOfWork;

namespace Idea.Cookbook.Services
{
    internal class IngredientService : IIngredientService
    {
        private readonly IUnitOfWorkFactory _factory;

        private readonly IRepository<Ingredient, Guid> _repository;

        public IngredientService(
            IUnitOfWorkFactory factory,
            IRepository<Ingredient, Guid> repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public Task<Ingredient> CreateIngredientAsync(Ingredient ingredient) =>
            _factory.With(_repository).CreateAndCommitAsync(ingredient);

        public async Task<Ingredient> UpdateIngredientAsync(Guid id, Ingredient unit)
        {
            using (var uow = _factory.Create())
            {
                var entity = await new FetchIngredient(id).FetchAsync(_factory);
                entity.Unit.Id = unit.UnitId;
                entity.Amount = unit.Amount;
                entity.Name = unit.Name;

                await _repository.UpdateAsync(entity);
                await uow.CommitAsync();

                return entity;
            }
        }

        public Task RemoveIngredientAsync(Guid id) => _factory.With(_repository).DeleteAndCommitAsync(id);

        public Task<IReadOnlyCollection<Ingredient>> GetIngredientsAsync() =>
            new ReadIngredients().ExecuteAsync(_factory);

        public Task<Ingredient> GetIngredientAsync(Guid id) => new FetchIngredient(id).FetchAsync(_factory);
    }
}