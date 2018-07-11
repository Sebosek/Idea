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
    public class RecipesController : ControllerBase
    {
        private readonly RecipeOrchestration _orchestration;

        public RecipesController(RecipeOrchestration orchestration)
        {
            _orchestration = orchestration;
        }

        [HttpGet]
        public Task<IReadOnlyCollection<Recipe>> GetAsync() =>  _orchestration.GetRecipesAsync();

        [HttpGet]
        [Route("{id}")]
        public Task<Recipe> GetAsync([FromRoute] Guid id) => _orchestration.GetRecipeAsync(id);

        [HttpGet]
        [Route("{id}/ingredients")]
        public Task<IReadOnlyCollection<Ingredient>> IngredientsAsync([FromRoute] Guid id) =>
            _orchestration.GetRecipeIngredientsAsync(id);

        [HttpPost]
        public Task<Recipe> CreateAsync(RecipeCreate model) => _orchestration.CreateRecipeAsync(model);

        [HttpPut]
        [Route("{id}")]
        public Task<Recipe> UpdateAsync([FromRoute] Guid id, [FromBody] RecipeUpdate model) => 
            _orchestration.UpdateRecipeAsync(id, model);

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync([FromRoute] Guid id) => _orchestration.RemoveRecipeAsync(id);
    }
}