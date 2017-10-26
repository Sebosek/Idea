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
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>
    {
        protected DbContext Context { get; private set; }

        public async Task<IReadOnlyCollection<TEntity>> ExecuteAsync(IUnitOfWork uow)
        {
            Context = uow.ToEntityFrameworkUnitOfWork<TDbContext>().DbContext;

            return await CreateQuery().ToListAsync();
        }

        protected abstract IQueryable<TEntity> CreateQuery();

        protected virtual IQueryable<TEntity> Map() => Context.Set<TEntity>();
    }

    public abstract class Query<TDbContext, TQueryData, TEntity, TKey> : IQuery<TEntity, TKey>
        where TDbContext : DbContext
        where TQueryData : IQueryData
        where TEntity : class, IEntity<TKey>
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

        protected virtual IQueryable<TEntity> Map() => Context.Set<TEntity>();
    }
}