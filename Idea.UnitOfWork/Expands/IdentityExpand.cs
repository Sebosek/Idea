using System;

using Idea.Entity;

namespace Idea.UnitOfWork.Expands
{
    public class IdentityExpand<TKey> : IEntityExpand<TKey>
    {
        private readonly IIdentityIdentifier<TKey> _identifier;

        public IdentityExpand(IIdentityIdentifier<TKey> identifier)
        {
            _identifier = identifier;
        }

        public void BeforeCreate(IEntity<TKey> entity)
        {
            ApplyIdentity(entity, (record, id) => record.CreatedBy = id);
        }

        public void BeforeUpdate(IEntity<TKey> entity)
        {
            ApplyIdentity(entity, (record, id) => record.UpdatedBy = id);
        }

        public void BeforeRemove(IEntity<TKey> entity)
        {
            ApplyIdentity(entity, (record, id) => record.RemovedBy = id);
        }

        private void ApplyIdentity(IEntity<TKey> entity, Action<Record<TKey>, TKey> apply)
        {
            if (_identifier == null)
            {
                return;
            }

            if (!(entity is Record<TKey> record))
            {
                return;
            }

            var id = _identifier.IdentityKey();
            apply(record, id);
        }
    }
}