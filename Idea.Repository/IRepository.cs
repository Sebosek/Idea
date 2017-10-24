using System.Threading.Tasks;

namespace Idea.Repository
{
    public interface IRepository<TEntity, in TKey>
    {
        Task<TEntity> FindAsync(TKey id);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}