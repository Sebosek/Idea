using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        void Add(IUnitOfWork uow);
        void Close();
        IUnitOfWork Current();
        bool CanCommit();
        Task CommitAllAsync();
        void CommitAll();
        void CleanUp();
    }
}