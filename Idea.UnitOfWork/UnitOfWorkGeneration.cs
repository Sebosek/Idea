using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public class UnitOfWorkGeneration : IUnitOfWorkGeneration
    {
        protected static int RANGE = 32;

        private int _height;
        private int _index;
        private IUnitOfWork[] _stack;

        protected int Index => _index;
        protected IUnitOfWork[] Stack => _stack;

        public bool AllClosed => _index == 0;

        public UnitOfWorkGeneration()
        {
            _index = 0;
            _stack = new IUnitOfWork[RANGE];
        }

        public void Add(IUnitOfWork uow)
        {
            _stack[_height++] = uow;
            _index++;
        }

        public bool CanCommit()
        {
            var i = 0;
            while (i < RANGE && _stack[i] != null)
            {
                if (_stack[i].IsCommited)
                {
                    return true;
                }
                i++;
            }

            return false;
        }

        public void Commit()
        {
            var i = 0;
            while (i < RANGE && _stack[i] != null)
            {
                if (_stack[i].IsCommited)
                {
                    var uow = _stack[i] as UnitOfWork;
                    if (uow != null)
                    {
                        uow.DoCommit();
                    }
                }
                i++;
            }
        }

        public async Task CommitAsync()
        {
            var i = 0;
            while (i < RANGE && _stack[i] != null)
            {
                if (_stack[i].IsCommited)
                {
                    var uow = _stack[i] as UnitOfWork;
                    if (uow != null)
                    {
                        await uow.DoCommitAsync();
                    }
                }
                i++;
            }
        }

        public void CloseCurrent()
        {
            _index--;
            if (_index < 0)
            {
                throw new Exception("None Unit of Work is currently open.");
            }

            var uow = _stack[_index] as UnitOfWork;
            if (uow != null)
            {
                uow.IsOpen = false;
            }
        }

        public IUnitOfWork Current()
        {
            return _stack[_height - 1];
        }

        public void CleanUp()
        {
            var j = 0;
            while (j < RANGE && _stack[j] != null)
            {
                Release(_stack[j]);
                _stack[j] = null;
                j++;
            }
        }

        protected virtual void Release(IUnitOfWork uow) { }
    }
}
