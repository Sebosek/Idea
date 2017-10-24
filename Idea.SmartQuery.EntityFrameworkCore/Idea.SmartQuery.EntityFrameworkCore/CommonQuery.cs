using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.SmartQuery.EntityFrameworkCore.Extensions;
using Idea.SmartQuery.Interfaces;
using Idea.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace Idea.SmartQuery.EntityFrameworkCore
{
    public class CommonQuery<TDbContext, TEntity, TKey> : IQuery<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TDbContext : DbContext
    {
        protected readonly Expression<Func<TEntity, object>>[] Includes;

        protected readonly Expression<Func<TEntity, bool>> Filter;

        private DbContext Context { get; set; }

        public CommonQuery()
            : this(e => true, new Expression<Func<TEntity, object>>[0])
        {
        }

        public CommonQuery(params Expression<Func<TEntity, object>>[] includes)
            : this(e => true, new Expression<Func<TEntity, object>>[0])
        {
            Includes = includes;
        }

        public CommonQuery(Expression<Func<TEntity, bool>> filter)
            : this(filter, new Expression<Func<TEntity, object>>[0])
        {
        }

        public CommonQuery(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            Filter = filter;
            Includes = includes;
        }

        public async Task<IReadOnlyCollection<TEntity>> ExecuteAsync(IUnitOfWork uow)
        {
            Context = uow.ToEntityFrameworkUnitOfWork<TDbContext>().DbContext;

            return await Map().ToListAsync();
        }

        protected virtual IQueryable<TEntity> Map() =>
            Includes.Aggregate(Context.Set<TEntity>().Where(Filter), (current, i) => current.Include(i));
    }

    public class CommonQuery<TDbContext, TOrderBy, TEntity, TKey> : CommonQuery<TDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TDbContext : DbContext
    {
        protected readonly Expression<Func<TEntity, TOrderBy>> Order;

        protected readonly int Take = int.MaxValue;

        protected readonly int Skip = 0;

        public CommonQuery(Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order, int skip, int take)
            : this(filter, order, skip, take, new Expression<Func<TEntity, object>>[0])
        {
        }

        public CommonQuery(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderBy>> order, int skip,
            int take, params Expression<Func<TEntity, object>>[] includes)
            : base(filter, includes)
        {
            Order = order;
            Skip = skip;
            Take = take;
        }

        public new async Task<IReadOnlyCollection<TEntity>> ExecuteAsync(IUnitOfWork uow)
        {
            uow.CheckEntityFrameworkUnitOfWork<TDbContext>();

            return await Map().OrderBy(Order).Skip(Skip).Take(Take).ToListAsync();
        }
    }
}