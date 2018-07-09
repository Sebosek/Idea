using System;
using System.Linq;

using Idea.Entity;
using Idea.UnitOfWork.EntityFrameworkCore.Enums;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public interface IModelContext : IDisposable
    {
        IQueryable<TEntity> Set<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>;

        RemoveStrategy AppliedRemoveStrategy();
    }
}