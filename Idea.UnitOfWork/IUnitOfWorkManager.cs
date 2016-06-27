namespace Idea.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        void Add(IUnitOfWork uow);
        void Close();
        IUnitOfWork Current();

        bool CanCommit();
    }
}