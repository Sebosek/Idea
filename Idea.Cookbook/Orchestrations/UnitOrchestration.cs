using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Idea.Cookbook.Extensions;
using Idea.Cookbook.Interfaces.Services;
using Idea.Cookbook.Models;

namespace Idea.Cookbook.Orchestrations
{
    public class UnitOrchestration
    {
        private readonly IUnitService _service;

        private readonly IMapper _mapper;

        public UnitOrchestration(IUnitService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<Unit>> GetUnitsAsync() =>
            _mapper.Map<IReadOnlyCollection<Unit>>(await _service.GetUnitsAsync().ConfigureAwait(false));

        public Task<Unit> CreateUnitAsync(UnitCreate unit) =>
            _mapper.Bridge<Unit, Entities.Unit>(() => _service.CreateUnitAsync(_mapper.Map<Entities.Unit>(unit)));

        public Task<Unit> UpdateUnitAsync(Guid id, UnitUpdate unit) =>
            _mapper.Bridge<Unit, Entities.Unit>(() => _service.UpdateUnitAsync(id, _mapper.Map<Entities.Unit>(unit)));

        public Task RemoveUnitAsync(Guid id) => _service.RemoveUnitAsync(id);
    }
}