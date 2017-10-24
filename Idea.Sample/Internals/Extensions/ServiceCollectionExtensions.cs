using System;

using Idea.Repository;
using Idea.Repository.EntityFrameworkCore;
using Idea.Sample.Internals.Configurations;
using Idea.Sample.Internals.DbContext;
using Idea.Sample.Internals.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Idea.Sample.Internals.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSample(this IServiceCollection services)
        {
            services.AddSingleton(AutomapperConfiguration.Configuration());

            services.AddScoped<IRepository<Post, Guid>, Repository<SampleDbContext, Post, Guid>>();
            services.AddScoped<IRepository<Tag, Guid>, Repository<SampleDbContext, Tag, Guid>>();
            services.AddScoped<IRepository<PostTag, Guid>, Repository<SampleDbContext, PostTag, Guid>>();

            return services;
        }
    }
}