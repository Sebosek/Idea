using System;

using Idea.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Idea.UnitOfWork.EntityFrameworkCore.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder Record<TRecord, TKey>(
            this ModelBuilder builder,
            Action<EntityTypeBuilder<TRecord>> action)
            where TRecord : class, IRecord<TKey> =>
            Record<TRecord, TKey>(builder).Entity(action);

        public static ModelBuilder Record<TRecord, TKey>(
            this ModelBuilder builder)
            where TRecord : class, IRecord<TKey> =>
            builder
                .Entity<TRecord>(e => {
                    e.HasKey(k => k.Id);
                    e.HasIndex(i => i.Removed);
                });

        public static ModelBuilder Entity<TEntity, TKey>(
            this ModelBuilder builder,
            Action<EntityTypeBuilder<TEntity>> action)
            where TEntity : class, IEntity<TKey> =>
            Entity<TEntity, TKey>(builder).Entity(action);

        public static ModelBuilder Entity<TEntity, TKey>(this ModelBuilder builder)
            where TEntity : class, IEntity<TKey> =>
            builder.Entity<TEntity>(e => e.HasKey(k => k.Id));
    }
}