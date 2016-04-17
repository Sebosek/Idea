using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Idea7.UnitOfWork.Tests")]
namespace Idea7.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWorkManager _manager;
        private bool _isDisposed = false;
        private bool _isOpen = false;

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
        protected virtual void DoRollback() { }

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

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
        }

        public override bool Equals(object obj)
        {
            var argument = obj as IUnitOfWork;
            return argument != null && Id.Equals(argument.Id);
        }
    }
}
