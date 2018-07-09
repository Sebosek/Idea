using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Idea.Entity;
using Idea.UnitOfWork.EntityFrameworkCore.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWork<TDbContext, TKey> : UnitOfWork
        where TDbContext : DbContext, IModelContext
    {
        private readonly IEnumerable<IEntityExpand<TKey>> _expands;

        public UnitOfWork(TDbContext context, IUnitOfWorkManager manager, IEnumerable<IEntityExpand<TKey>> expands)
            : base(manager)
        {
            _expands = expands;
            ModelContext = context;
        }

        public TDbContext ModelContext { get; }

        public void Release() => ModelContext.Dispose();

        protected override Task DoCommitAsync()
        {
            ApplyRecordTracking();
            return ModelContext.SaveChangesAsync(default(CancellationToken));
        }

        private void ApplyRecordTracking()
        {
            var entries = ModelContext.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                TrackCreated(entry);
                TrackUpdated(entry);
                TrackRemoved(entry);
            }
        }

        private void TrackCreated(EntityEntry entry)
        {
            if (entry.State != EntityState.Added)
            {
                return;
            }

            if (!(entry.Entity is IEntity<TKey> entity))
            {
                return;
            }

            foreach (var expand in _expands)
            {
                expand.BeforeCreate(entity);
            }
        }

        private void TrackUpdated(EntityEntry entry)
        {
            if (entry.State != EntityState.Modified)
            {
                return;
            }

            if (!(entry.Entity is IEntity<TKey> entity))
            {
                return;
            }

            foreach (var expand in _expands)
            {
                expand.BeforeCreate(entity);
            }
        }

        private void TrackRemoved(EntityEntry entry)
        {
            if (entry.State != EntityState.Deleted || ModelContext.AppliedRemoveStrategy() == RemoveStrategy.Drop)
            {
                return;
            }

            if (!(entry.Entity is IEntity<TKey> entity))
            {
                return;
            }

            foreach (var expand in _expands)
            {
                expand.BeforeRemove(entity);
            }

            if (entry.Entity is Record<TKey> record)
            {
                entry.State = EntityState.Modified;
                CascadeRemove(record);
            }
        }

        private void CascadeRemove(Record<TKey> entity)
        {
            var entry = ModelContext.ChangeTracker.Entries().FirstOrDefault(f => f.Entity == entity);
            var navigation = entry.Navigations.Where(w => w is CollectionEntry).Cast<CollectionEntry>();

            foreach (var item in navigation)
            {
                if (item.CurrentValue == null)
                {
                    continue;
                }

                var entities = item.CurrentValue.Cast<Record<TKey>>().Where(w => w != null);
                foreach (var inner in entities)
                {
                    foreach (var expand in _expands)
                    {
                        expand.BeforeRemove(inner);
                    }

                    CascadeRemove(inner);
                }
            }
        }
    }
}
