using System.Threading.Tasks;

using Idea.Entity;
using Idea.Repository;
using Idea.UnitOfWork;

namespace Idea.NetCore.EntityFrameworkCore
{
    public static class UnitOfWorkFactoryExtensions
    {
        public static WithRepositoryBuilder<TEntity, TKey> With<TEntity, TKey>(
            this IUnitOfWorkFactory factory,
            IRepository<TEntity, TKey> repository)
            where TEntity : IEntity<TKey>
        {
            return new WithRepositoryBuilder<TEntity, TKey>(factory, repository);
        }
    }
}