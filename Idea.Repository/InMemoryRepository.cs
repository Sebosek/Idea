using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public IReadOnlyCollection<TEntity> Get<TOrderBy>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order,
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return Data.AsQueryable().Where(filter).OrderBy(order).Skip(skip).Take(take).ToList();
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

        public Task<IReadOnlyCollection<TEntity>> GetAsync<TOrderBy>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderBy>> order, int skip, int take, params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(
                Data.AsQueryable().Where(filter).OrderBy(order).Skip(skip).Take(take) as IReadOnlyCollection<TEntity>);
        }
    }
}
