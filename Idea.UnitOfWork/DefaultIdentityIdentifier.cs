namespace Idea.UnitOfWork
{
    public class DefaultIdentityIdentifier<TKey> : IIdentityIdentifier<TKey>
    {
        public TKey IdentityKey() => default(TKey);
    }
}