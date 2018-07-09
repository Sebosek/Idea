using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;

namespace Idea.Cookbook.Interfaces.Services
{
    public interface IUnitService
    {
        Task<Unit> CreateUnitAsync(Unit unit);

        Task<Unit> UpdateUnitAsync(Guid id, Unit unit);

        Task RemoveUnitAsync(Guid id);

        Task<IReadOnlyCollection<Unit>> GetUnitsAsync();

        Task<Unit> GetUnitAsync(Guid id);
    }
}