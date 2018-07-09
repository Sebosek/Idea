using System;
using System.Threading.Tasks;

using Idea.UnitOfWork.Exceptions;

namespace Idea.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private const string NOT_OPEN_UOW = "No unit of work is currently open.";

        private const string WRONG_UOW = "Attempt to commit the outer unit of work.";

        private readonly IUnitOfWorkManager _manager;

        private bool _isDisposed;

        public UnitOfWork(IUnitOfWorkManager manager)
        {
            Id = Guid.NewGuid();

            _manager = manager;
            _manager.Add(this);
            IsOpen = true;
        }

        public Guid Id { get; protected internal set; }

        public bool IsCommited { get; protected internal set; }

        protected internal bool IsOpen { get; set; }

        public Task CommitAsync()
        {
            CheckOpenedUow();
            CheckCurrentUow();

            IsCommited = true;
            IsOpen = false;

            if (_manager.CanCommit())
            {
                return _manager.CommitAllAsync();
            }

            return Task.CompletedTask;
        }

        public async Task RollbackAsync()
        {
            CheckOpenedUow();
            CheckCurrentUow();

            await DoRollbackAsync();
            IsOpen = false;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (IsOpen)
            {
                RollbackAsync().GetAwaiter().GetResult();
            }

            _manager.Close();
            _manager.CleanUp();
        }

        public override bool Equals(object obj)
        {
            return obj is IUnitOfWork argument && Id.Equals(argument.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected virtual Task DoCommitAsync() => Task.CompletedTask;

        protected virtual Task DoRollbackAsync() => Task.CompletedTask;

        protected internal Task InternalCommitAsync() => DoCommitAsync();

        protected internal Task InternalRollbackAsync() => DoRollbackAsync();

        private void CheckOpenedUow()
        {
            if (!IsOpen)
            {
                throw new NoOpenedUnitOfWorkException(NOT_OPEN_UOW);
            }
        }

        private void CheckCurrentUow()
        {
            var last = _manager.Current();
            if (!last.Equals(this))
            {
                throw new CommitOuterUnitOfWorkException(WRONG_UOW);
            }
        }
    }
}
