using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private const int Depth = 32;

        private string _id;
        private int _index;
        
        private readonly IUnitOfWork[] _stack;

        protected internal IUnitOfWork[] Stack => _stack;

        public UnitOfWorkManager()
        {
            _index = 0;
            _stack = new IUnitOfWork[Depth];
            _id = Guid.NewGuid().ToString();
        }
        
        public void Add(IUnitOfWork uow)
        {
            if (_index >= Depth)
            {
                throw new Exception("Reached maximum Unit of work depth!");
            }

            _stack[_index++] = uow;
        }

        public bool CanCommit()
        {
            if (_index > 1)
            {
                return false;
            }

            int i = _index;
            while (_stack[i] != null && i < Depth)
            {
                if (!_stack[i].IsCommited)
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        public void Close()
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
            var index = _index - 1;
            if (index < 0 || _stack[index] == null)
            {
                throw new Exception("None Unit of Work is currently open.");
            }

            return _stack[index];
        }

        public void CommitAll()
        {
            // get top
            var top = 0;
            for (int i = 0; i < Depth; i++)
            {
                if (_stack[i] == null) break;
                top++;
            }

            // commiting
            for (int i = top - 1; i >= 0; i--)
            {
                if (_stack[i].IsCommited)
                {
                    var uow = _stack[i] as UnitOfWork;
                    if (uow != null)
                    {
                        uow.DoCommit();
                    }
                }
            }
        }

        public async Task CommitAllAsync()
        {
            // get top
            var top = 0;
            for (int i = 0; i < Depth; i++)
            {
                if (_stack[i] == null) break;
                top++;
            }

            // commiting
            for (int i = top - 1; i >= 0; i--)
            {
                if (_stack[i].IsCommited)
                {
                    var uow = _stack[i] as UnitOfWork;
                    if (uow != null)
                    {
                        await uow.DoCommitAsync();
                    }
                }
            }
        }

        public void CleanUp()
        {
            if (_index > 0) return;

            var i = 0;
            while (_stack[i] != null)
            {
                _stack[i] = null;
                i++;
            }
        }
    }
}