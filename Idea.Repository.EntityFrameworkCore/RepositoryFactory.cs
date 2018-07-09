using Idea.Entity;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

namespace Idea.Repository.EntityFrameworkCore
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RepositoryFactory(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public IRepository<TEntity, TKey> CreateRepository<TDbContext, TEntity, TKey>()
            where TDbContext : ModelContext<TKey>
            where TEntity : class, IEntity<TKey>
        {
            return new Repository<TDbContext, TEntity, TKey>(_unitOfWorkManager);
        }
    }
}