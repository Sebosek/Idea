using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Idea7.Entity;
using Idea7.Query;
using Idea7.UnitOfWork;

namespace Idea7.Repository.EntityFramework6
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

        public virtual long Count(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return query.Count(_database.AsQueryable());
        }

        public virtual IEnumerable<TEntity> Fetch(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return query.Fetch(_database.AsQueryable());
        }

        public virtual TEntity FetchOne(IQueryObject<TEntity> query)
        {
            ResolveUnitOfWork();
            return query.FetchOne(_database.AsQueryable());
        }

        public virtual TEntity Find(TKey id)
        {
            ResolveUnitOfWork();
            return _database.Find(id);
        }

        public virtual void Create(TEntity entity)
        {
            ResolveUnitOfWork();
            _database.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            ResolveUnitOfWork();

            _database.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
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

            var uow = _manager.Current() as UnitOfWork.EntityFramework6.UnitOfWork;
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            _context = uow.DbContext;
            _database = _context.Set<TEntity>();
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
    }
}
