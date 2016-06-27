using System;

namespace Idea.Entity
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