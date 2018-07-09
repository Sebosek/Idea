namespace Idea.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();

        IDataProvider DataProvider();
    }
}