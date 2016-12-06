using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Idea.Entity;
using Idea.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using EFUnitOfWork = Idea.UnitOfWork.EntityFrameworkCore.UnitOfWork;

namespace Idea.Query.EntityFrameworkCore
{
    public abstract class Query<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected DbContext Context { get; private set; }

        protected abstract IQueryable<TEntity> CreateQuery();

        public IReadOnlyCollection<TEntity> Execute(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work.");
            }

            var input = uow as EFUnitOfWork;
            if (input == null)
            {
                throw new Exception("Given Unit of work can not be used in Entity Framework Query.");
            }

            Context = input.DbContext;
            return new ReadOnlyCollection<TEntity>(CreateQuery().ToList());
        }
    }
}