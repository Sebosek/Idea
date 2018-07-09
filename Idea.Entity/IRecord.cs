using System;

namespace Idea.Entity
{
    public interface IRecord<out TKey> : IEntity<TKey>
    {
        DateTime Created { get; }

        TKey CreatedBy { get; }

        DateTime? Updated { get; }

        TKey UpdatedBy { get; }

        DateTime? Removed { get; }

        TKey RemovedBy { get; }
    }
}