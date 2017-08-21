using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private const string NOT_OPEN_UOW = "No unit of work is currently open.";

        private const string WRONG_UOW = "Try to commit outside Unit of work.";

        private readonly IUnitOfWorkManager _manager;

        private bool _isDisposed;

        private bool _isOpen;

        private bool _isCommited;

        public UnitOfWork(IUnitOfWorkManager manager)
        {
            Id = Guid.NewGuid().ToString();

            _manager = manager;
            _manager.Add(this);
            _isOpen = true;
        }

        public string Id { get; }

        public bool IsCommited => _isCommited;

        protected internal bool IsOpen
        {
            get => _isOpen;
            set => _isOpen = value;
        }

        public void Commit()
        {
            CheckOpenedUow();
            CheckCurrentUow();

            _isCommited = true;
            _isOpen = false;

            if (_manager.CanCommit())
            {
                _manager.CommitAll();
            }
        }

        public Task CommitAsync()
        {
            CheckOpenedUow();
            CheckCurrentUow();

            _isCommited = true;
            _isOpen = false;

            if (_manager.CanCommit())
            {
                return _manager.CommitAllAsync();
            }

            return Task.FromResult(false);
        }

        public void Rollback()
        {
            CheckOpenedUow();
            CheckCurrentUow();

            DoRollback();
            _isOpen = false;
        }

        public Task RollbackAsync()
        {
            CheckOpenedUow();
            CheckCurrentUow();

            var task = DoRollbackAsync();
            _isOpen = false;
            return task;
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
                Rollback();
            }

            _manager.Close();
            _manager.CleanUp();
        }

        public override bool Equals(object obj)
        {
            var argument = obj as IUnitOfWork;
            return argument != null && Id.Equals(argument.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected internal virtual void DoCommit() { }

        protected internal virtual Task DoCommitAsync() { return Task.FromResult(false); }

        protected internal virtual void DoRollback() { }

        protected internal virtual Task DoRollbackAsync() { return Task.FromResult(false); }

        private void CheckOpenedUow()
        {
            if (!IsOpen)
            {
                throw new Exception(NOT_OPEN_UOW);
            }
        }

        private void CheckCurrentUow()
        {
            var last = _manager.Current();
            if (!last.Equals(this))
            {
                throw new Exception(WRONG_UOW);
            }
        }
    }
}
