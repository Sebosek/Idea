using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;
using Idea.Query.EntityFrameworkCore;
using Idea.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace Idea.Cookbook.Queries
{
    public class ReadRecipes : Query<Recipe, Guid>
    {
        protected override Task<IReadOnlyCollection<Recipe>> QueryAsync(IDataProvider provider) =>
            Task.FromResult<IReadOnlyCollection<Recipe>>(
                provider.Data<Recipe, Guid>().Include(i => i.Ingredients).ThenInclude(i => i.Unit).ToList());
    }
}