using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.UnitOfWork;

namespace Idea.SmartQuery.Interfaces
{
    public interface IQuery<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        Task<IReadOnlyCollection<TEntity>> ExecuteAsync(IUnitOfWork uow);
    }
}