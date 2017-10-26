using System;
using System.Linq.Expressions;
using Idea.Entity;
using Idea.SmartQuery.Interfaces;

namespace Idea.SmartQuery.QueryData
{
    public class GenericConditions<TOrderBy, TEntity, TKey> : GenericConditions<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        public Expression<Func<TEntity, TOrderBy>> Order { get; }

        public int Take { get; }

        public int Skip { get; }

        public GenericConditions(Expression<Func<TEntity, TOrderBy>> order,
            params Expression<Func<TEntity, object>>[] includes)
            : this(w => true, order, 0, int.MaxValue, includes)
        {
        }

        public GenericConditions(Expression<Func<TEntity, TOrderBy>> order, int skip, int take,
            params Expression<Func<TEntity, object>>[] includes)
            : this(w => true, order, skip, take, includes)
        {
        }

        public GenericConditions(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderBy>> order, int skip, int take, params Expression<Func<TEntity, object>>[] includes)
            : base(filter, includes)
        {
            Order = order;
            Skip = skip;
            Take = take;
        }
    }

    public class GenericConditions<TEntity, TKey> : IQueryData
        where TEntity : IEntity<TKey>
    {
        public Expression<Func<TEntity, object>>[] Includes { get; }

        public Expression<Func<TEntity, bool>> Filter { get; }

        public GenericConditions(params Expression<Func<TEntity, object>>[] includes)
            : this(w => true, includes)
        {
        }

        public GenericConditions(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            Filter = filter;
            Includes = includes;
        }
    }
}