using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.UnitOfWork;

namespace Idea.Repository.EntityFramework6
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IUnitOfWorkManager _manager;
        private DbSet<TEntity> _database;
        private DbContext _context;

        protected DbSet<TEntity> Database => _database;
        protected DbContext Context => _context;

        public Repository(IUnitOfWorkManager manager)
        {
            _manager = manager;
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

        public Task<TEntity> FindAsync(TKey id)
        {
            ResolveUnitOfWork();
            return _database.SingleOrDefaultAsync(s => s.Id.Equals(id));
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

        protected void ResolveUnitOfWork()
        {
            var uow = _manager.Current() as UnitOfWork.EntityFramework6.UnitOfWork;
            if (uow == null)
            {
                throw new Exception("Unable to resolve Entity Framework Unit of work");
            }

            _context = uow.DbContext;
            _database = _context.Set<TEntity>();
        }
    }
}
