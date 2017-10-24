using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Idea.Entity;

namespace Idea.Repository
{
    /// <summary>
    /// Abstract in-memory repository
    /// </summary>
    public abstract class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public abstract IList<TEntity> Data { get; }

        public Task<TEntity> FindAsync(TKey id)
        {
            return Task.FromResult<TEntity>(Data.SingleOrDefault(s => s.Id.Equals(id)));
        }

        public Task CreateAsync(TEntity entity)
        {
            return Task.Factory.StartNew(() => Data.Add(entity));
        }

        public Task UpdateAsync(TEntity entity)
        {
            return Task.Factory.StartNew(() =>
            {
                Data.Remove(entity);
                Data.Add(entity);
            });
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.Factory.StartNew(() => Data.Remove(entity));
        }
    }
}
