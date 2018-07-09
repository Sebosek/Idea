using System.Threading.Tasks;

using Idea.Entity;

namespace Idea.Repository
{
    public interface IRepository<TEntity, in TKey>
        where TEntity : IEntity<TKey>
    {
        Task<TEntity> FindAsync(TKey id);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}