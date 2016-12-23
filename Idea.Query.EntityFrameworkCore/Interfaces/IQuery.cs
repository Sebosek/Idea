using System.Collections.Generic;

using Idea.Entity;
using Idea.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace Idea.Query.EntityFrameworkCore.Interfaces
{
    public interface IQuery<TDbContext, out TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TDbContext : DbContext
    {
        IReadOnlyCollection<TEntity> Execute(IUnitOfWork uow);
    }
}