using System;
using System.Collections.Generic;

namespace Idea7.UnitOfWork
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

            _stack[_index] = null;
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
    }
}