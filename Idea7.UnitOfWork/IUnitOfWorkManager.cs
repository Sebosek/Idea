using System.Collections.Generic;

namespace Idea7.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        Stack<IUnitOfWork> Stack { get; }
        void Add(IUnitOfWork uow);
        void Close();
        IUnitOfWork Current();
    }
}