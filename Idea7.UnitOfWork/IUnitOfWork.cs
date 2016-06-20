using System;
using System.Threading.Tasks;

namespace Idea7.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        string Id { get; }

        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}