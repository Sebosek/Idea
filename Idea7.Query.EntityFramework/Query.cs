using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

using Idea7.Entity;
using Idea7.Query.EntityFramework.Utils;
using Idea7.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace Idea7.Query.EntityFramework
{
    public abstract class Query<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        private readonly IUnitOfWorkManager _manager;
        private readonly IList<Tuple<Expression<Func<TEntity, object>>, Order>> _orderByCollection;
        private int _skip;
        private int _take;

        protected Query(IUnitOfWorkManager manager)
        {
            Context = ResolveUnitOfWork(manager);
            _orderByCollection = new List<Tuple<Expression<Func<TEntity, object>>, Order>>();
        }

        protected DbContext Context { get; }

        public int Skip
        {
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _skip = value;
            }
        }

        public int Take
        {
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _skip = value;
            }
        }

        protected abstract IQueryable<TEntity> CreateQuery();

        public void AddOrderBy(string property, Order by)
        {
            _orderByCollection.Add(
                new Tuple<Expression<Func<TEntity, object>>, Order>(
                    QueryExtensions.CreateExpression<TEntity>(property), by));
        }

        public void AddOrderBy(Expression<Func<TEntity, object>> order, Order by)
        {
            _orderByCollection.Add(new Tuple<Expression<Func<TEntity, object>>, Order>(order, by));
        }

        public IReadOnlyCollection<TEntity> Execute()
        {
            return new ReadOnlyCollection<TEntity>(ApplyOrderBy(CreateQuery()).Skip(_skip).Take(_take).ToList());
        }

        private DbContext ResolveUnitOfWork(IUnitOfWorkManager manager)
        {
            if (Context != null)
            {
                return Context;
            }

            var uow = manager.Current() as UnitOfWork.EntityFramework.UnitOfWork;
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            return uow.DbContext;
        }

        private IQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query)
        {
            if (_orderByCollection != null && _orderByCollection.Count > 0)
            {
                var reverse = _orderByCollection.Reverse();
                foreach (var order in reverse)
                {
                    query = order.Item2 == Order.Asc ? query.OrderBy(order.Item1) : query.OrderByDescending(order.Item1);
                }
            }
            else
            {
                query = query.OrderBy(o => o.Id);
            }

            return query;
        }
    }
}