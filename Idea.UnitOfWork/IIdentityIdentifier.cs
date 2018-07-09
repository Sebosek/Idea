namespace Idea.UnitOfWork
{
    public interface IIdentityIdentifier<out TKey>
    {
        TKey IdentityKey();
    }
}