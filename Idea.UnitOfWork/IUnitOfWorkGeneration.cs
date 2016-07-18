using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public interface IUnitOfWorkGeneration
    {
        bool AllClosed { get; }

        void Add(IUnitOfWork uow);
        bool CanCommit();
        void Commit();
        Task CommitAsync();
        void CloseCurrent();
        IUnitOfWork Current();
        void CleanUp();
    }
}
