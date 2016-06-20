using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Idea7.UnitOfWork.Tests")]
namespace Idea7.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWorkManager _manager;
        private bool _isDisposed;
        private bool _isOpen;

        public UnitOfWork(IUnitOfWorkManager manager)
        {
            Id = Guid.NewGuid().ToString();

            _manager = manager;
            _manager.Add(this);
            _isOpen = true;
        }

        protected internal bool IsOpen => _isOpen;
        public string Id { get; }

        protected virtual void DoCommit() { }
        protected virtual Task DoCommitAsync() { return Task.FromResult(false); }
        protected virtual void DoRollback() { }
        protected virtual Task DoRollbackAsync() { return Task.FromResult(false); }

        public void Commit()
        {
            if (IsOpen)
            {
                var last = _manager.Current();
                if (!last.Equals(this))
                {
                    throw new Exception("Try to commit outside Unit of work.");
                }

                DoCommit();

                _isOpen = false;
            }
            else
            {
                throw new Exception("Unit of work isn't open.");
            }
        }

        public Task CommitAsync()
        {
            if (IsOpen)
            {
                var last = _manager.Current();
                if (!last.Equals(this))
                {
                    throw new Exception("Try to commit outside Unit of work.");
                }

                var task = DoCommitAsync();
                _isOpen = false;
                return task;
            }

            throw new Exception("Unit of work isn't open.");
        }

        public void Rollback()
        {
            if (IsOpen)
            {
                var last = _manager.Current();
                if (!last.Equals(this))
                {
                    throw new Exception("Try to rollback outside Unit of work.");
                }

                DoRollback();
                _isOpen = false;
            }
            else
            {
                throw new Exception("Unit of work isn't open.");
            }
        }

        public Task RollbackAsync()
        {
            if (IsOpen)
            {
                var last = _manager.Current();
                if (!last.Equals(this))
                {
                    throw new Exception("Try to rollback outside Unit of work.");
                }

                var task = DoRollbackAsync();
                _isOpen = false;
                return task;
            }

            throw new Exception("Unit of work isn't open.");
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            if (IsOpen)
            {
                Rollback();
            }

            _manager.Close();
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
    }
}
