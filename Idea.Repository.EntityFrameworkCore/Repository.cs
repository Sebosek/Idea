using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

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

        public IReadOnlyCollection<TEntity> Get<TOrderBy>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order,
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes)
        {
            ResolveUnitOfWork();
            var query = _database.Where(filter).OrderBy(order).Skip(skip).Take(take);

            return includes.Aggregate(query, (current, i) => current.Include(i)).ToList();
        }

        public Task<TEntity> FindAsync(TKey id)
        {
            ResolveUnitOfWork();
            return _database.FindAsync(id);
        }

        public Task CreateAsync(TEntity entity)
        {
            ResolveUnitOfWork();
            return _database.AddAsync(entity);
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

        public Task<IReadOnlyCollection<TEntity>> GetAsync<TOrderBy>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TOrderBy>> order,
            int skip,
            int take,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return Task<IReadOnlyCollection<TEntity>>.Factory.StartNew(() =>
            {
                ResolveUnitOfWork();
                var query = _database.Where(filter).OrderBy(order).Skip(skip).Take(take);

                return includes.Aggregate(query, (current, i) => current.Include(i)).ToList();
            });
        }

        protected void ResolveUnitOfWork()
        {
            if (!(_manager.Current() is UnitOfWork<TDbContext> uow))
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            _context = uow.DbContext;
            _database = _context.Set<TEntity>();
        }
    }
}
