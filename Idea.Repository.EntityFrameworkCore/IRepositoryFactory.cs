using Idea.Entity;
using Idea.UnitOfWork.EntityFrameworkCore;

namespace Idea.Repository.EntityFrameworkCore
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity, TKey> CreateRepository<TDbContext, TEntity, TKey>()
            where TDbContext : ModelContext<TKey>
            where TEntity : class, IEntity<TKey>;
    }
}