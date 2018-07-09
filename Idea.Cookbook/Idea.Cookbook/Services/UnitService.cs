using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;
using Idea.Cookbook.Interfaces.Services;
using Idea.Cookbook.Queries;
using Idea.NetCore.EntityFrameworkCore;
using Idea.Repository;
using Idea.UnitOfWork;

namespace Idea.Cookbook.Services
{
    internal class UnitService : IUnitService
    {
        private readonly IRepository<Unit, Guid> _repository;

        private readonly IUnitOfWorkFactory _factory;

        public UnitService(
            IRepository<Unit, Guid> repository,
            IUnitOfWorkFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public Task<Unit> CreateUnitAsync(Unit unit) => _factory.With(_repository).CreateAndCommitAsync(unit);

        public Task<Unit> UpdateUnitAsync(Guid id, Unit unit) =>
            _factory.With(_repository).ComulativeUpdateAndCommitAsync(id,
                u => u.Symbol = unit.Symbol,
                u => u.Name = unit.Name);

        public Task RemoveUnitAsync(Guid id) => _factory.With(_repository).DeleteAndCommitAsync(id);

        public Task<IReadOnlyCollection<Unit>> GetUnitsAsync() => new ReadUnits().ExecuteAsync(_factory);

        public Task<Unit> GetUnitAsync(Guid id) => _factory.With(_repository).FindAsync(id);
    }
}