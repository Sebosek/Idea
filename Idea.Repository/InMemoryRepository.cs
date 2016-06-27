using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.Query;

namespace Idea.Repository
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

        public Task<long> CountAsync(IQueryObject<TEntity> query)
        {
            return Task.FromResult(query.Count(Data.AsQueryable()));
        }

        public Task<IEnumerable<TEntity>> FetchAsync(IQueryObject<TEntity> query)
        {
            return Task.FromResult(query.Fetch(Data.AsQueryable()));
        }

        public Task<TEntity> FetchOneAsync(IQueryObject<TEntity> query)
        {
            return Task.FromResult(query.FetchOne(Data.AsQueryable()));
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
