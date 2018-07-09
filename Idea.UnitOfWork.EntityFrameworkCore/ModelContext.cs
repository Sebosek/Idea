using System.Linq;
using System.Reflection;

using Idea.Entity;
using Idea.UnitOfWork.EntityFrameworkCore.Enums;

using Microsoft.EntityFrameworkCore;

namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public abstract class ModelContext<TKey> : DbContext, IModelContext
    {
        public ModelContext(DbContextOptions options)
            : base(options)
        {
        }

        public abstract RemoveStrategy AppliedRemoveStrategy();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DbModel(modelBuilder);

            if (AppliedRemoveStrategy() == RemoveStrategy.Drop)
            {
                return;
            }

            var entities = modelBuilder.Model.GetEntityTypes();
            var methodInfo = RemovedFilterMethodInfo();
            var records = entities.Where(f => f.ClrType.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == typeof(IRecord<>)));

            foreach (var record in records)
            {
                var method = methodInfo.MakeGenericMethod(record.ClrType);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }

        protected abstract void DbModel(ModelBuilder builder);

        private void RemovedFilter<T>(ModelBuilder modelBuilder)
            where T : class, IRecord<TKey> =>
            modelBuilder.Entity<T>().HasQueryFilter(f => f.Removed == null);

        private MethodInfo RemovedFilterMethodInfo() =>
            typeof(ModelContext<TKey>).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(t => t.IsGenericMethod && t.Name == nameof(RemovedFilter));

        public IQueryable<TEntity> Set<TEntity, TKeyEntity>()
            where TEntity : class, IEntity<TKeyEntity> => Set<TEntity>();
    }
}