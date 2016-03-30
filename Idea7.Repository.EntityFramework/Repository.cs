using System;
using System.Collections.Generic;
using System.Linq;

using Idea7.Entity;
using Idea7.Query;
using Idea7.UnitOfWork;

using Microsoft.Data.Entity;

namespace Idea7.Repository.EntityFramework
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IUnitOfWorkManager _manager;
        private DbSet<TEntity> _database;
        private DbContext _context;

        public Repository(IUnitOfWorkManager manager)
        {
            _manager = manager;
        }

        public long Count(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return query.Count(_database.AsQueryable());
        }

        public IEnumerable<TEntity> Fetch(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return query.Fetch(_database.AsQueryable());
        }

        public TEntity FetchOne(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return query.FetchOne(_database.AsQueryable());
        }

        public TEntity Find(TKey id)
        {
            ResolveUnitOfWork();
            return _database.SingleOrDefault(s => s.Id.Equals(id));
        }

        public void Create(TEntity entity)
        {
            ResolveUnitOfWork();
            _database.Add(entity);
        }

        public void Update(TEntity entity)
        {
            ResolveUnitOfWork();

            _database.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            ResolveUnitOfWork();
            _database.Remove(entity);
        }
        
        public void ResolveUnitOfWork()
        {
            if (_context != null && _database != null)
            {
                return;
            }

            var uow = _manager.Current() as UnitOfWork.EntityFramework.UnitOfWork;
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            _context = uow.DbContext;
            _database = _context.Set<TEntity>();
        }
    }
}
