using System;

namespace Idea7.Entity
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
        DateTime Created { get; }
        DateTime? Storno { get; }
    }
}