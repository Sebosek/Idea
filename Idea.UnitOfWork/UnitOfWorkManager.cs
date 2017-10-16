using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        internal const int DEPTH = 32;

        private readonly IUnitOfWorkGenerationFactory _factory;

        private readonly IUnitOfWorkGeneration[] _stack;

        private int _generation;

        public UnitOfWorkManager(IUnitOfWorkGenerationFactory factory)
        {
            _factory = factory;
            _generation = 0;
            _stack = new IUnitOfWorkGeneration[DEPTH];
        }

        public void Add(IUnitOfWork uow)
        {
            if (_generation >= DEPTH)
            {
                throw new Exception("Reached maximum Unit of work depth!");
            }

            var current = _generation;
            if (_stack[current] == null)
            {
                _stack[current] = _factory.Create();
            }

            _stack[current].Add(uow);
            _generation++;
        }

        public bool CanCommit()
        {
            if (_generation > 1)
            {
                return false;
            }

            var previous = true;
            var i = _generation;

            while (i < DEPTH && _stack[i] != null)
            {
                if (!previous)
                {
                    return false;
                }

                previous = _stack[i].CanCommit();
                i++;
            }

            return true;
        }

        public void Close()
        {
            var current = _generation - 1;
            if (current < 0)
            {
                throw new Exception("None Unit of Work is currently open.");
            }

            _stack[current].CloseCurrent();
            if (!_stack[current].AllClosed)
            {
                return;
            }

            _generation--;
            if (_generation < 0)
            {
                throw new Exception("None Unit of Work is currently open.");
            }
        }

        public IUnitOfWork Current()
        {
            var index = _generation - 1;
            if (index < 0 || _stack[index] == null)
            {
                throw new Exception("None Unit of Work is currently open.");
            }

            return _stack[index].Current();
        }

        public void CommitAll()
        {
            // get top
            var top = 0;
            for (var i = 0; i < DEPTH; i++)
            {
                if (_stack[i] == null)
                {
                    break;
                }

                top++;
            }

            // committing
            for (var i = top - 1; i >= 0; i--)
            {
                if (_stack[i].CanCommit())
                {
                    _stack[i].Commit();
                }
            }
        }

        public async Task CommitAllAsync()
        {
            // get top
            var top = 0;
            for (var i = 0; i < DEPTH; i++)
            {
                if (_stack[i] == null)
                {
                    break;
                }

                top++;
            }

            // committing
            for (var i = top - 1; i >= 0; i--)
            {
                if (_stack[i].CanCommit())
                {
                    await _stack[i].CommitAsync();
                }
            }
        }

        public void CleanUp()
        {
            if (_generation > 0)
            {
                return;
            }

            var i = 0;
            while (_stack[i] != null)
            {
                _stack[i].CleanUp();
                _stack[i] = null;
                i++;
            }
        }
    }
}