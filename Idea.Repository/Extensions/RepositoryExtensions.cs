using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Idea.Entity;

namespace Idea.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static IReadOnlyCollection<TEntity> GetAll<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity<TKey>
        {
            return repository.Get(f => true, o => o.Id, 0, int.MaxValue, includes);
        }

        public static IReadOnlyCollection<TEntity> Get<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IEntity<TKey>
        {
            return repository.Get(filter, o => o.Id, 0, int.MaxValue);
        }

        public static IReadOnlyCollection<TEntity> Get<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity<TKey>
        {
            return repository.Get(filter, o => o.Id, 0, int.MaxValue, includes);
        }

        public static IReadOnlyCollection<TEntity> Get<TEntity, TKey, TOrderBy>(
            this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order,
            int skip,
            int take)
            where TEntity : class, IEntity<TKey>
        {
            return repository.Get(filter, order, skip, take);
        }

        public static Task<IReadOnlyCollection<TEntity>> GetAllAsync<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity<TKey>
        {
            return repository.GetAsync(f => true, o => o.Id, 0, int.MaxValue, includes);
        }

        public static Task<IReadOnlyCollection<TEntity>> GetAsync<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IEntity<TKey>
        {
            return repository.GetAsync(filter, o => o.Id, 0, int.MaxValue);
        }

        public static Task<IReadOnlyCollection<TEntity>> GetAsync<TEntity, TKey>(
            this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity<TKey>
        {
            return repository.GetAsync(filter, o => o.Id, 0, int.MaxValue, includes);
        }

        public static Task<IReadOnlyCollection<TEntity>> GetAsync<TEntity, TKey, TOrderBy>(
            this IRepository<TEntity, TKey> repository,
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order,
            int skip,
            int take)
            where TEntity : class, IEntity<TKey>
        {
            return repository.GetAsync(filter, order, skip, take);
        }
    }
}