using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;
using Idea.Query.EntityFrameworkCore;
using Idea.UnitOfWork;

namespace Idea.Cookbook.Queries
{
    public class ReadUnits : Query<Unit, Guid>
    {
        protected override Task<IReadOnlyCollection<Unit>> QueryAsync(IDataProvider provider) =>
            Task.FromResult<IReadOnlyCollection<Unit>>(provider.Data<Unit, Guid>().ToList());
    }
}