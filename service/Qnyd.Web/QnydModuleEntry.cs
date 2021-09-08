using Ao.Lang;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Qnyd.Data;
using Structing;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Qnyd.Web
{
    internal class QnydModuleEntry : AutoModuleEntity
    {
        public override int Order => 23333;
        public override void ReadyRegister(IRegisteContext context)
        {
            var config = context.GetConfiguration();
            var mongoSection = config.GetSection("Mongodb");
            var mongodbCon = mongoSection["ConnectionString"];
            var maxConn = mongoSection.GetValue<int>("MaxConnectionPoolSize");
            var minConn = mongoSection.GetValue<int>("MinConnectionPoolSize");
            if (maxConn <= 0)
            {
                maxConn = 500;
            }
            if (minConn <= 0)
            {
                minConn = 50;
            }
            var setting = MongoClientSettings.FromConnectionString(mongodbCon);
            setting.MaxConnectionPoolSize = maxConn;
            setting.MinConnectionPoolSize = minConn;
            var client = new MongoClient(setting);
            context.Services.AddSingleton(client);
            context.Services.AddSingleton<IMongoClient>(client);
        }
        public override Task StartAsync(IServiceProvider provider)
        {
            var hostBuilder = provider.GetWebHostBuilder();
            hostBuilder.ConfigureAppConfiguration((ctx, builder) =>
            {
                var basePath = ctx.HostingEnvironment.ContentRootPath;
                if (Debugger.IsAttached)
                {
                    basePath = AppDomain.CurrentDomain.BaseDirectory;
                }
                var path = Path.Combine(basePath, "Settings");
                if (Directory.Exists(path))
                {
                    var jsonFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
                    foreach (var item in jsonFiles)
                    {
                        builder.AddJsonFile(item, true, true);
                    }
                }
            });
            return base.StartAsync(provider);
        }
        public override void Register(IRegisteContext context)
        {
            var config = context.GetConfiguration();
            AddDb(context.Services, config);
            AddLang(context.Services);
            AddSwagger(context.Services);
            base.Register(context);
        }
        public override Task BeforeReadyAsync(IReadyContext context)
        {
            var app = context.GetApplicationBuilder();
            var isDevelopment = context.GetIsDevelopment();
            if (isDevelopment.GetValueOrDefault(false))
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Qyad.Web v1"));
            app.UseResponseCompression();
            //app.UseHttpsRedirection();
            return base.BeforeReadyAsync(context);
        }
        public override Task ReadyAsync(IReadyContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseStaticFiles();
            app.UseRouting();
            return base.ReadyAsync(context);
        }
        public override Task AfterReadyAsync(IReadyContext context)
        {
            var app = context.GetApplicationBuilder();
            var spaSection = context.Configuration.GetSection("SPA");
            var enableSPA = spaSection.GetValue<bool>("Enable");
            if (enableSPA)
            {
                var isListen = spaSection.GetValue<bool>("IsListen");
                if (!isListen)
                {
                    var distPath = spaSection.GetValue<string>("DistPath");
                    var path = context.GetWebRootPath();
                    var target = Path.Combine(path, distPath);
                    var provider = new PhysicalFileProvider(target);
                    var opt = new StaticFileOptions
                    {
                        FileProvider = provider
                    };
                    app.UseSpaStaticFiles(opt);
                }
                else
                {
                    var proxyUri = spaSection.GetValue<string>("ListenUrl");
                    app.UseSpa(b =>
                    {
                        b.UseProxyToSpaDevelopmentServer(proxyUri);
                    });
                }
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return base.AfterReadyAsync(context);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Qyad.Web", Version = "v1" });
            });
        }
        private void AddDb(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<QnydDbContext>((provider, builder) =>
            {
#if DEBUG
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                builder.UseLoggerFactory(loggerFactory);
#endif
                var config = provider.GetRequiredService<IConfiguration>();
                var connectionString = config["Database:ConnectionString"];
                builder.UseSqlite(connectionString);
            })
            .AddIdentity<QnydUser, QnydRole>(d =>
            {
                configuration.GetSection("Password").Bind(d.Password);
            })
            .AddEntityFrameworkStores<QnydDbContext>();
        }
        private void AddLang(IServiceCollection services)
        {
            var langSer = new LanguageService();
            var stringPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Strings");
            if (Directory.Exists(stringPath))
            {
                var dirs = Directory.EnumerateDirectories(stringPath);
                foreach (var item in dirs)
                {
                    var dirName = Path.GetFileName(item);
                    var m = new DefaultLanguageMetadata(new CultureInfo(dirName));
                    var fileProv = new PhysicalFileProvider(item);
                    var fs = Directory.EnumerateFiles(item);
                    foreach (var f in fs)
                    {
                        var fn = Path.GetFileName(f);
                        m.Add(new JsonConfigurationSource
                        {
                            FileProvider = fileProv,
                            Path = fn
                        });
                    }
                    langSer.Add(m);
                }
            }

            services.AddSingleton<ILanguageService>(langSer);
            services.AddScoped(provider =>
            {
                var ls = provider.GetRequiredService<ILanguageService>();
                return ls.GetCurrentRoot();
            });
        }
    }
}
