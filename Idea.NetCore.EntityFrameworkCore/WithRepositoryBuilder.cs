using System;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.Repository;
using Idea.UnitOfWork;

namespace Idea.NetCore.EntityFrameworkCore
{
    public class WithRepositoryBuilder<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        private readonly IUnitOfWorkFactory _factory;

        private readonly IRepository<TEntity, TKey> _repository;

        public WithRepositoryBuilder(IUnitOfWorkFactory factory, IRepository<TEntity, TKey> repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public async Task<TEntity> CreateAndCommitAsync(TEntity entity)
        {
            using (var uow = _factory.Create())
            {
                await _repository.CreateAsync(entity).ConfigureAwait(false);
                await uow.CommitAsync().ConfigureAwait(false);

                return entity;
            }
        }

        public async Task DeleteAndCommitAsync(TKey key)
        {
            using (var uow = _factory.Create())
            {
                var entíty = await _repository.FindAsync(key).ConfigureAwait(false);
                await _repository.DeleteAsync(entíty).ConfigureAwait(false);

                await uow.CommitAsync().ConfigureAwait(false);
            }
        }

        public async Task<TEntity> FindAsync(TKey key)
        {
            using (_factory.Create())
            {
                return await _repository.FindAsync(key).ConfigureAwait(false);
            }
        }

        public async Task<TEntity> ComulativeUpdateAndCommitAsync(TKey key, params Action<TEntity>[] updates)
        {
            using (var uow = _factory.Create())
            {
                var result = await _repository.ComulativeUpdateAsync(key, updates).ConfigureAwait(false);
                await uow.CommitAsync().ConfigureAwait(false);

                return result;
            }
        }
    }
}