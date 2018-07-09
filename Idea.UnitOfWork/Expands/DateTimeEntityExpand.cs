using System;

using Idea.Entity;

namespace Idea.UnitOfWork.Expands
{
    public class DateTimeEntityExpand<TKey> : IEntityExpand<TKey>
    {
        public void BeforeCreate(IEntity<TKey> entity)
        {
            ApplyDateTime(entity, e => e.Created = DateTime.UtcNow);
        }

        public void BeforeUpdate(IEntity<TKey> entity)
        {
            ApplyDateTime(entity, e => e.Updated = DateTime.UtcNow);
        }

        public void BeforeRemove(IEntity<TKey> entity)
        {
            ApplyDateTime(entity, e => e.Removed = DateTime.UtcNow);
        }

        private void ApplyDateTime(IEntity<TKey> entity, Action<Record<TKey>> apply)
        {
            if (!(entity is Record<TKey> record))
            {
                return;
            }
            
            apply(record);
        }
    }
}