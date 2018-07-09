using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Cookbook.Models;
using Idea.Cookbook.Orchestrations;

using Microsoft.AspNetCore.Mvc;

namespace Idea.Cookbook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly UnitOrchestration _orchestration;

        public UnitsController(UnitOrchestration orchestration)
        {
            _orchestration = orchestration;
        }

        [HttpGet]
        public Task<IReadOnlyCollection<Unit>> GetAsync() =>  _orchestration.GetUnitsAsync();

        [HttpPost]
        public Task<Unit> CreateAsync(UnitCreate model) => _orchestration.CreateUnitAsync(model);

        [HttpPut]
        [Route("{id}")]
        public Task<Unit> UpdateAsync([FromRoute] Guid id, [FromBody] UnitUpdate model) => _orchestration.UpdateUnitAsync(id, model);

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync([FromRoute] Guid id) => _orchestration.RemoveUnitAsync(id);
    }
}