using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Idea.Repository
{
    public interface IRepository<TEntity, in TKey>
    {
        TEntity Find(TKey id);

        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        IReadOnlyCollection<TEntity> Get<TOrderBy>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order,
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> FindAsync(TKey id);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<IReadOnlyCollection<TEntity>> GetAsync<TOrderBy>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order,
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes);
    }
}