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
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientOrchestration _orchestration;

        public IngredientsController(IngredientOrchestration orchestration)
        {
            _orchestration = orchestration;
        }

        [HttpGet]
        public Task<IReadOnlyCollection<Ingredient>> GetAsync() =>  _orchestration.GetIngredientsAsync();

        [HttpPost]
        public Task<Ingredient> CreateAsync(IngredientCreate model) => _orchestration.CreateIngredientAsync(model);

        [HttpPut]
        [Route("{id}")]
        public Task<Ingredient> UpdateAsync([FromRoute] Guid id, [FromBody] IngredientUpdate model) => 
            _orchestration.UpdateIngredientAsync(id, model);

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync([FromRoute] Guid id) => _orchestration.RemoveIngredientAsync(id);
    }
}