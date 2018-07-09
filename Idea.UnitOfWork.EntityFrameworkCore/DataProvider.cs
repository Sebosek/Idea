using System.Linq;

using Idea.Entity;

using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class DataProvider<TDbContext> : IDataProvider
        where TDbContext : DbContext, IModelContext
    {
        private readonly IDbContextFactory<TDbContext> _factory;

        public DataProvider(IDbContextFactory<TDbContext> factory)
        {
            _factory = factory;
        }

        public IQueryable<TEntity> Data<TEntity, TKey>()
            where TEntity : class, IEntity<TKey> =>
            _factory.CreateDbContext().Set<TEntity, TKey>();
    }
}