using System.Threading.Tasks;

namespace Idea7.Repository
{
    public interface IRepository<TEntity, in TKey> : IQueryExecuter<TEntity>
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