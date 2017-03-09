using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

using Idea.Entity;
using Idea.SmartQuery.Interfaces;
using Idea.UnitOfWork;

namespace Idea.SmartQuery.EntityFramework6
{
    public abstract class Query<TEntity, TKey> : IQuery<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected Query(IQueryReader<IQueryData> reader)
        {
            Reader = reader;
        }

        protected IQueryReader<IQueryData> Reader { get; }

        protected DbContext Context { get; private set; }

        public IReadOnlyCollection<TEntity> Execute(IUnitOfWork uow)
        {
            var input = uow as UnitOfWork.EntityFramework6.UnitOfWork;
            if (input == null)
            {
                throw new ArgumentException("Given Unit of work can not be used in Entity Framework Query.");
            }

            Context = input.DbContext;
            return new ReadOnlyCollection<TEntity>(CreateQuery().ToList());
        }

        protected abstract IQueryable<TEntity> CreateQuery();

        protected virtual IQueryable<TMapEntity> Map<TMapEntity>()
            where TMapEntity : class, IEntity<TKey>
        {
            return Context.Set<TMapEntity>();
        }
    }
}