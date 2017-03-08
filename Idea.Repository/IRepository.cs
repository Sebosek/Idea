using System.Threading.Tasks;

namespace Idea.Repository
{
    public interface IRepository<TEntity, in TKey>
    {
        TEntity Find(TKey id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<TEntity> FindAsync(TKey id);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}