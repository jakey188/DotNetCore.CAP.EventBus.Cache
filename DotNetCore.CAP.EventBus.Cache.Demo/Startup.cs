using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Data;
using DotNetCore.CAP.Cap.EventBus.Cache.Services;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Caches;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Consumers;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Users.Caches;
using CSRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var csredis = new CSRedisClient("127.0.0.1");
            RedisHelper.Initialization(csredis);
            services.TryAddSingleton<CSRedisClient>(x =>
            {
                return csredis;
            });

            services.AddSingleton<IMongoDatabase>((provider) =>
            {
                IMongoClient client = new MongoClient("mongodb://localhost:27017");
                return client.GetDatabase("CacheEvent");
            });

            services.AddTransient<CacheEventConsumer>();
            services.AddTransient<IConsumer, RoleCacheEventConsumer>();
            services.AddTransient<IConsumer, UserCacheEventConsumer>();

            services.AddCap(options =>
            {
                options.ConsumerThreadCount = 1;//CAP消费者线程
                options.UseDashboard();
                options.UseRabbitMQ("127.0.0.1");
                options.UseMongoDB(c => {
                    c.DatabaseName = "CacheEventCap";
                    c.DatabaseConnection = "mongodb://localhost:27017";
                });
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
