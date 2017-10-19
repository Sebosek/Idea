using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Idea.Entity;
using Idea.SmartQuery.Interfaces;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace Idea.SmartQuery.EntityFrameworkCore
{
    public abstract class Query<TDbContext, TEntity, TKey> : IQuery<TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TDbContext : DbContext
    {
        protected Query(IQueryReader<IQueryData> reader)
        {
            Reader = reader;
        }

        protected Query()
        {
        }

        protected IQueryReader<IQueryData> Reader { get; }

        protected DbContext Context { get; private set; }

        public IReadOnlyCollection<TEntity> Execute(IUnitOfWork uow)
        {
            if (!(uow is UnitOfWork<TDbContext> input))
            {
                throw new ArgumentException("Given Unit of work can not be used in Entity Framework Query.");
            }

            Context = input.DbContext;
            return new ReadOnlyCollection<TEntity>(CreateQuery().ToList());
        }

        protected abstract IQueryable<TEntity> CreateQuery();

        protected virtual IQueryable<TMapEntity> Map<TMapEntity>()
            where TMapEntity : class, IEntity<TKey> => Context.Set<TMapEntity>();
    }
}