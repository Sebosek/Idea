using System.Threading.Tasks;

namespace Idea.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        void Add(UnitOfWork uow);

        void Close();

        UnitOfWork Current();

        bool CanCommit();

        Task CommitAllAsync();

        void CleanUp();
    }
}