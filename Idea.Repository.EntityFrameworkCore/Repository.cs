using System;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace Idea.Repository.EntityFrameworkCore
{
    public class Repository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TDbContext : ModelContext<TKey>
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

        protected void ResolveUnitOfWork()
        {
            if (!(_manager.Current() is UnitOfWork<TDbContext, TKey> uow))
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            _context = uow.ModelContext;
            _database = _context.Set<TEntity>();
        }
    }
}
