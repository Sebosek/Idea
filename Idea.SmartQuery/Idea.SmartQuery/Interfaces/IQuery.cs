using System.Collections.Generic;

using Idea.Entity;
using Idea.UnitOfWork;

namespace Idea.SmartQuery.Interfaces
{
    public interface IQuery<out TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        IReadOnlyCollection<TEntity> Execute(IUnitOfWork uow);
    }
}