using System.Linq;

using Idea.Entity;

namespace Idea.UnitOfWork
{
    public interface IDataProvider
    {
        IQueryable<TEntity> Data<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>;
    }
}