using System;

namespace Idea.Entity
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Canceled { get; set; }
    }
}