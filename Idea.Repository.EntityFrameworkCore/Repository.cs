using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.Query;
using Idea.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace Idea.Repository.EntityFrameworkCore
{
    public class Repository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TDbContext : DbContext
    {
        private readonly IUnitOfWorkManager _manager;

        private DbSet<TEntity> _database;

        private DbContext _context;

        public Repository(IUnitOfWorkManager manager)
        {
            _manager = manager;
        }

        protected DbSet<TEntity> Database => _database;

        protected DbContext Context => _context;

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
            return _database.Find(id);
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

        public Task<TEntity> FindAsync(TKey id)
        {
            ResolveUnitOfWork();
            return _database.FindAsync(id);
        }

        public Task CreateAsync(TEntity entity)
        {
            return Task.Factory.StartNew(() =>
            {
                ResolveUnitOfWork();
                _database.Add(entity);
            });
        }

        public Task UpdateAsync(TEntity entity)
        {
            return Task.Factory.StartNew(() =>
            {
                ResolveUnitOfWork();

                _database.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            });
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.Factory.StartNew(() =>
            {
                ResolveUnitOfWork();
                _database.Remove(entity);
            });
        }

        public Task<long> CountAsync(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return Task.FromResult(query.Count(_database.AsQueryable()));
        }

        public Task<IEnumerable<TEntity>> FetchAsync(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return Task.FromResult(query.Fetch(_database.AsQueryable()));
        }

        public Task<TEntity> FetchOneAsync(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return Task.FromResult(query.FetchOne(_database.AsQueryable()));
        }

        protected void ResolveUnitOfWork()
        {
            var uow = _manager.Current() as UnitOfWork.EntityFrameworkCore.UnitOfWork<TDbContext>;
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            _context = uow.DbContext;
            _database = _context.Set<TEntity>();
        }
    }
}
