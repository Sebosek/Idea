namespace Idea.Entity
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}