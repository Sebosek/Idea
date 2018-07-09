using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Guid Id { get; }

        bool IsCommited { get; }
        
        Task CommitAsync();
        
        Task RollbackAsync();
    }
}