using System;
using System.Linq;

using Idea.Entity;
using Microsoft.EntityFrameworkCore;
using EFUnitOfWork = Idea.UnitOfWork.EntityFrameworkCore.UnitOfWork;

namespace Idea.Command.EntityFrameworkCore
{
    public abstract class CommandQuery<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        protected DbContext Context { get; private set; }

        protected abstract IQueryable<TEntity> ProcessCommand();

        public void Execute(EFUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            Context = uow.DbContext;
            ProcessCommand();
        }
    }
}
