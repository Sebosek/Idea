using System;
using System.Collections.Generic;

namespace Idea7.UnitOfWork
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private string _id;
        
        private readonly Stack<IUnitOfWork> _stack;

        public UnitOfWorkManager()
        {
            _stack = new Stack<IUnitOfWork>();
            _id = Guid.NewGuid().ToString();
        }

        public Stack<IUnitOfWork> Stack => _stack;

        public void Add(IUnitOfWork uow)
        {
            Stack.Push(uow);
        }

        public void Close()
        {
            if (Stack.Count == 0)
            {
                throw new Exception("None Unit of Work is currently open.");
            }
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