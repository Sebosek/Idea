using System;

using Idea.Entity;
using Idea.Repository;
using Idea.Repository.EntityFrameworkCore;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;
using Idea.UnitOfWork.Expands;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using UnitOfWorkGenerationFactory = Idea.UnitOfWork.UnitOfWorkGenerationFactory;

namespace Idea.NetCore.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdea<TDbContext, TKey>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
            where TDbContext : DbContext, IModelContext =>
            services
                .AddDbContext<TDbContext>(options)
                .AddScoped<IDbContextFactory<TDbContext>>(_ => new DbContextFactory<TDbContext>(options))
                .AddScoped<IUnitOfWorkManager, UnitOfWorkManager>()
                .AddScoped<IUnitOfWorkGenerationFactory, UnitOfWorkGenerationFactory>()
                .AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory<TDbContext, TKey>>()
                .AddScoped<IDataProvider, DataProvider<TDbContext>>()
                .AddScoped<IRepositoryFactory, RepositoryFactory>()
                .AddIdentityIdentifier<DefaultIdentityIdentifier<TKey>, TKey>()
                .AddEntityExpand<DateTimeEntityExpand<TKey>, TKey>()
                .AddEntityExpand<IdentityExpand<TKey>, TKey>();

        public static IServiceCollection AddRepository<TModelContext, TEntity, TKey>(this IServiceCollection services)
            where TEntity : class, IEntity<TKey> 
            where TModelContext : ModelContext<TKey> =>
            services.AddScoped<IRepository<TEntity, TKey>, Repository<TModelContext, TEntity, TKey>>();

        public static IServiceCollection AddEntityExpand<TEntityExpand, TKey>(this IServiceCollection services)
            where TEntityExpand : class, IEntityExpand<TKey> =>
            services.AddTransient<IEntityExpand<TKey>, TEntityExpand>();

        public static IServiceCollection AddIdentityIdentifier<TIdentity, TKey>(this IServiceCollection services)
            where TIdentity : class, IIdentityIdentifier<TKey> =>
            services.AddTransient<IIdentityIdentifier<TKey>, TIdentity>();
    }
}