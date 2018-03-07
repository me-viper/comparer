using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Autofac;

using ComparerService.App.Services;
using ComparerService.App.Utility;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using ServiceStack.Redis;

using Swashbuckle.AspNetCore.Swagger;

namespace ComparerService.App
{
    public class Startup
    {
        private ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFacotry)
        {
            Configuration = configuration;
            _loggerFactory = loggerFacotry ?? NullLoggerFactory.Instance;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                p =>
                {
                    p.SwaggerDoc("v1", new Info { Title = "Comparer API", Version = "v1" });

                    var docPath = Path.Combine(AppContext.BaseDirectory, "ComparerService.App.xml");
                    p.IncludeXmlComments(docPath);
                });

            services.AddMvc().AddJsonOptions(p => p.SerializerSettings.Formatting = Formatting.Indented);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<DiffService>().As<IDiffService>();

            if (string.Equals(Configuration["store"], "redis", StringComparison.OrdinalIgnoreCase))
            {
                var redisConnectionString = "redis://localhost:6379";

                if (!string.IsNullOrWhiteSpace(Configuration["redis.endpoint"]))
                    redisConnectionString = Configuration["redis.endpoint"];

                builder.Register<IRedisClientsManager>(p => new BasicRedisClientManager(redisConnectionString));
                builder.RegisterType<RedisRepository>().As<IComparisonContentRepository>();

                _loggerFactory.CreateLogger<Startup>().LogInformation("Using Redis storage with endpoint {0}", redisConnectionString);
            }
            else
            {
                _loggerFactory.CreateLogger<Startup>().LogInformation("Using in-memory storage");
                builder.RegisterType<InMemoryRepository>().As<IComparisonContentRepository>().SingleInstance();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorLogging();

            app.UseSwagger();
            app.UseSwaggerUI(p => p.SwaggerEndpoint("/swagger/v1/swagger.json", "Comparer API v1"));

            app.UseMvc();
        }
    }
}
