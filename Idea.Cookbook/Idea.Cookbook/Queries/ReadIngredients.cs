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
    public class ReadIngredients : Query<Ingredient, Guid>
    {
        protected override Task<IReadOnlyCollection<Ingredient>> QueryAsync(IDataProvider provider) =>
            Task.FromResult<IReadOnlyCollection<Ingredient>>(
                provider.Data<Ingredient, Guid>().Include(i => i.Unit).ToList());
    }
}