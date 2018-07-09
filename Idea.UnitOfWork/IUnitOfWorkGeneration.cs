using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    /// <summary>
    /// Represent a collection of <see cref="UnitOfWork"/> in same generation.
    /// </summary>
    public interface IUnitOfWorkGeneration
    {
        bool AllClosed { get; }

        void Add(UnitOfWork uow);

        bool CanCommit();

        Task CommitAsync();

        void CloseCurrent();

        UnitOfWork Current();

        void CleanUp();
    }
}
