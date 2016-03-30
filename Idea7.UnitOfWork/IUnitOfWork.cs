using System;

namespace Idea7.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        string Id { get; }

        void Commit();
        void Rollback();
    }
}