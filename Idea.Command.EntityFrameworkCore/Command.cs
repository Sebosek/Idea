using System;
using System.Linq;

using Idea.Entity;
using Idea.UnitOfWork.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace Idea.Command.EntityFrameworkCore
{
    public abstract class CommandQuery<TDbContext, TEntity, TKey>
        where TEntity : IEntity<TKey>
        where TDbContext : DbContext
    {
        protected DbContext Context { get; private set; }

        protected abstract IQueryable<TEntity> ProcessCommand();

        public void Execute(UnitOfWork<TDbContext> uow)
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
