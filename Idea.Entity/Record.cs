using System;

namespace Idea.Entity
{
    public class Record<TKey> : Entity<TKey>, IRecord<TKey>
    {
        public DateTime Created { get; set; }

        public TKey CreatedBy { get; set; }

        public DateTime? Removed { get; set; }

        public TKey RemovedBy { get; set; }

        public DateTime? Updated { get; set; }

        public TKey UpdatedBy { get; set; }
    }
}