using System.Collections.Generic;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.UnitOfWork;

namespace Idea.Query.EntityFrameworkCore
{
    public abstract class Query<TEntity, TKey> : IQuery<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public Task<IReadOnlyCollection<TEntity>> ExecuteAsync(IUnitOfWorkFactory factory) => QueryAsync(factory.DataProvider());

        protected abstract Task<IReadOnlyCollection<TEntity>> QueryAsync(IDataProvider provider);
    }
}