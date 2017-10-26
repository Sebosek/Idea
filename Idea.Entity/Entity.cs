using System;

namespace Idea.Entity
{
    public class Entity<TKey> : IEntity<TKey>
    {
        public Entity()
        {
            Created = DateTime.UtcNow;
        }

        public TKey Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Canceled { get; set; }
    }
}