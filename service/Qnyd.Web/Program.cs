using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NLog.Web.AspNetCore;
using Qnyd.User;
using Structing.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Qnyd.Web
{
    public class Program
    {
        private static readonly ModuleCollection moduleEntries = new ModuleCollection
        {
            new QnydModuleEntry(),
            new UserModuleEntry()
        };

        internal static IReadOnlyList<IModuleEntry> ModuleEntries => moduleEntries;

        public static void Main(string[] args)
        {
            moduleEntries.Sort((x, y) => y.Order - x.Order);

            NLogBuilder.ConfigureNLog("NLog.config");
            var hostBuilder = CreateHostBuilder(args).Build();
            var lifetime = hostBuilder.Services.GetService<IHostApplicationLifetime>();
            lifetime.ApplicationStopping.Register(Close, hostBuilder.Services);
            hostBuilder.Run();
        }
        private static void Close(object status)
        {
            var provider = (IServiceProvider)status;
            using (var scope = provider.CreateScope())
            {
                var tasks = moduleEntries.Select(x => x.StopAsync(provider)).ToArray();
                Task.WhenAll(tasks).GetAwaiter().GetResult();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.AddNLog("NLog.config");
                builder.AddApplicationInsights();
            })
            .ConfigureWebHostDefaults((webBuilder) =>
            {
                var provider = new ValueServiceProvider
                {
                    Factories =
                    {
                        [typeof(IWebHostBuilder)] = _ => webBuilder
                    }
                };
                var tasks = ModuleEntries.Select(x => x.StartAsync(provider)).ToArray();
                Task.WhenAll(tasks).GetAwaiter().GetResult();

                webBuilder.UseStartup<Startup>();
            }).UseNLog();
    }
}
