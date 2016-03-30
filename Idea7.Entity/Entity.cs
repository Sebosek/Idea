using System;

namespace Idea7.Entity
{
    public class Entity<TKey> : IEntity<TKey>
    {
        public Entity()
        {
            Created = DateTime.Now;
        }

        public TKey Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Storno { get; set; }
    }
}