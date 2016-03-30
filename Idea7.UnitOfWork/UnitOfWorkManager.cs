using System;
using System.Collections.Generic;
using System.Threading;

namespace Idea7.UnitOfWork
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly ThreadLocal<Stack<IUnitOfWork>> _stack;

        public UnitOfWorkManager()
        {
            _stack = new ThreadLocal<Stack<IUnitOfWork>>();
        }

        protected Stack<IUnitOfWork> Stack => _stack.Value;

        public void Add(IUnitOfWork uow)
        {
            Stack.Push(uow);
        }

        public void Close()
        {
            Stack.Pop();
        }

        public IUnitOfWork Current()
        {
            if (Stack.Count == 0)
            {
                throw new Exception("None Unit of Work is currently open.");
            }
            return Stack.Peek();
        }
    }
}