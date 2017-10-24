using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        string Id { get; }

        bool IsCommited { get; }
        
        Task CommitAsync();
        
        Task RollbackAsync();
    }
}