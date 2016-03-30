namespace Idea7.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        void Add(IUnitOfWork uow);
        void Close();
        IUnitOfWork Current();
    }
}