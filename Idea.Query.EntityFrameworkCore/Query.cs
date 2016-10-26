﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Idea.Entity;
using Microsoft.EntityFrameworkCore;
using EFUnitOfWork = Idea.UnitOfWork.EntityFrameworkCore.UnitOfWork;

namespace Idea.Query.EntityFrameworkCore
{
    public abstract class Query<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected DbContext Context { get; private set; }

        protected abstract IQueryable<TEntity> CreateQuery();

        public IReadOnlyCollection<TEntity> Execute(EFUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            Context = uow.DbContext;
            return new ReadOnlyCollection<TEntity>(CreateQuery().ToList());
        }
    }
}