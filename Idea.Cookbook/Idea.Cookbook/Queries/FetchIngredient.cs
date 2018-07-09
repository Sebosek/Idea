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
    public class FetchIngredient : Query<Ingredient, Guid>
    {
        private readonly Guid _id;

        public FetchIngredient(Guid id)
        {
            _id = id;
        }

        protected override Task<IReadOnlyCollection<Ingredient>> QueryAsync(IDataProvider provider) =>
            Task.FromResult<IReadOnlyCollection<Ingredient>>(
                provider.Data<Ingredient, Guid>().Where(w => w.Id == _id).Include(i => i.Unit).ToList());
    }
}