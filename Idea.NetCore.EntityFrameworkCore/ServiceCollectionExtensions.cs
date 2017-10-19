using System;

using Idea.SmartQuery;
using Idea.SmartQuery.Interfaces;
using Idea.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Idea.NetCore.EntityFrameworkCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdea<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
            where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options, ServiceLifetime.Transient);
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            services.AddScoped<IUnitOfWorkGenerationFactory, UnitOfWorkGenerationFactory>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWork.EntityFrameworkCore.UnitOfWorkFactory<TDbContext>>();
            services.AddScoped<IQueryFactory, QueryFactory>();

            return services;
        }
    }
}