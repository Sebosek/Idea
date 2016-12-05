using System;

namespace Idea.Entity
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
        DateTime Created { get; }
        DateTime? Canceled { get; }
    }
}