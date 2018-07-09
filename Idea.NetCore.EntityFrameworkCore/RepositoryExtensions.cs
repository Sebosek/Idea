using System;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.Repository;

namespace Idea.NetCore.EntityFrameworkCore
{
    public static class RepositoryExtensions
    {
        public static async Task<TEntity> ComulativeUpdateAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, TKey key, params Action<TEntity>[] updates)
            where TEntity : IEntity<TKey>
        {
            var entity = await repository.FindAsync(key);
            foreach (var update in updates)
            {
                update(entity);
            }

            await repository.UpdateAsync(entity);

            return entity;
        }
    }
}