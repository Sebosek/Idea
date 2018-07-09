using System;

using AutoMapper;

using Idea.Cookbook.Entities;
using Idea.Cookbook.Interfaces.Services;
using Idea.Cookbook.Orchestrations;
using Idea.Cookbook.Services;
using Idea.NetCore.EntityFrameworkCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Idea.Cookbook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddAutoMapper()
                .AddSwaggerGen(SetSwagger)
                .AddIdea<CookbookModelContext, Guid>(
                    a => a.UseSqlServer(Configuration.GetConnectionString("Cookbook")).EnableSensitiveDataLogging(true))
                .AddRepository<CookbookModelContext, Unit, Guid>()
                .AddRepository<CookbookModelContext, Ingredient, Guid>()
                .AddRepository<CookbookModelContext, Recipe, Guid>()
                .AddIdentityIdentifier<IdentityIdentifier, Guid>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<UnitOrchestration>()
                .AddScoped<IIngredientService, IngredientService>()
                .AddScoped<IngredientOrchestration>()
                .AddScoped<RecipeService>()
                .AddScoped<RecipeOrchestration>()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) =>
            Environment(app, env)
                .UseSwagger()
                .UseSwaggerUI(a =>
                {
                    a.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    a.RoutePrefix = string.Empty;
                })
                .UseHttpsRedirection()
                .UseMvc();

        private void SetSwagger(SwaggerGenOptions options) =>
            options.SwaggerDoc("v1", new Info
            {
                Title = "Idea cookbook",
                Version = "v1",
                Description = "Sample project for Idea data source."
            });

        private IApplicationBuilder Environment(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.ApplicationName == "dev")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            return app;
        }
    }
}
