using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.SmartQuery.EntityFrameworkCore.Extensions;
using Idea.SmartQuery.Interfaces;
using Idea.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace Idea.SmartQuery.EntityFrameworkCore
{
    public abstract class Query<TDbContext, TEntity, TKey> : IQuery<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TDbContext : DbContext
    {
        protected DbContext Context { get; private set; }

        public async Task<IReadOnlyCollection<TEntity>> ExecuteAsync(IUnitOfWork uow)
        {
            Context = uow.ToEntityFrameworkUnitOfWork<TDbContext>().DbContext;

            return await CreateQuery().ToListAsync();
        }

        protected abstract IQueryable<TEntity> CreateQuery();

        protected virtual IQueryable<TMapEntity> Map<TMapEntity>()
            where TMapEntity : class, IEntity<TKey> => Context.Set<TMapEntity>();
    }

    public abstract class Query<TQueryData, TDbContext, TEntity, TKey> : IQuery<TEntity, TKey>
        where TQueryData : IQueryData
        where TEntity : IEntity<TKey>
        where TDbContext : DbContext
    {
        protected Query(IQueryReader<TQueryData> reader)
        {
            Reader = reader;
        }

        protected IQueryReader<TQueryData> Reader { get; }

        protected DbContext Context { get; private set; }

        public async Task<IReadOnlyCollection<TEntity>> ExecuteAsync(IUnitOfWork uow)
        {
            Context = uow.ToEntityFrameworkUnitOfWork<TDbContext>().DbContext;

            return await CreateQuery().ToListAsync();
        }

        protected abstract IQueryable<TEntity> CreateQuery();

        protected virtual IQueryable<TMapEntity> Map<TMapEntity>()
            where TMapEntity : class, IEntity<TKey> => Context.Set<TMapEntity>();
    }
}