using System.Collections.Generic;
using System.Linq;

using Idea7.Entity;
using Idea7.Query;

namespace Idea7.Repository
{
    /// <summary>
    /// Abstract in-memory repository
    /// </summary>
    public abstract class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public abstract IList<TEntity> Data { get; }

        public long Count(IQueryObject<TEntity> query)
        {
            return query.Count(Data.AsQueryable());
        }

        public IEnumerable<TEntity> Fetch(IQueryObject<TEntity> query)
        {
            return query.Fetch(Data.AsQueryable());
        }

        public TEntity FetchOne(IQueryObject<TEntity> query)
        {
            return query.FetchOne(Data.AsQueryable());
        }

        public TEntity Find(TKey id)
        {
            return Data.SingleOrDefault(s => s.Id.Equals(id));
        }

        public void Create(TEntity entity)
        {
            Data.Add(entity);
        }

        public void Update(TEntity entity)
        {
            Delete(entity);
            Create(entity);
        }

        public void Delete(TEntity entity)
        {
            Data.Remove(entity);
        }
    }
}
