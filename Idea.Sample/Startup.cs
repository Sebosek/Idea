using System.Collections.Generic;
using System.Linq;

using Idea.NetCore.EntityFrameworkCore;
using Idea.Sample.Internals.DbContext;
using Idea.Sample.Internals.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Idea.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connections = ConnectionStrings(Configuration);

            services
                .AddIdea<SampleDbContext>(o => o.UseNpgsql(connections["Idea"]))
                .AddSample()
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }

        private static IDictionary<string, string> ConnectionStrings(IConfiguration configuration)
        {
            return configuration.GetSection("ConnectionStrings").AsEnumerable()
                .Where(s => s.Key.StartsWith("ConnectionStrings:"))
                .ToDictionary(s => s.Key.Replace("ConnectionStrings:", string.Empty), s => s.Value);
        }
    }
}
