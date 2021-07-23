using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ao.Lang;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Qnyd.Data;
using Structing.Core;

namespace Qnyd.Web
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
            var mvcBuilder=services.AddControllers();
            AddModules(services, mvcBuilder);

            services.AddResponseCompression();
            services.AddMemoryCache();

            services.AddLogging();
        }
        private void AddModules(IServiceCollection services,IMvcBuilder mvcBuilder)
        {
            var rgxCtx = new RegisteContext(services);
            rgxCtx.SetMvcBuilder(mvcBuilder);
            rgxCtx.SetConfiguration(Configuration);
            foreach (var item in Program.ModuleEntries)
            {
                item.ReadyRegister(rgxCtx);
            }
            foreach (var item in Program.ModuleEntries)
            {
                item.Register(rgxCtx);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            void Run(Func<IModuleEntry,Task> func)
            {
                var tasks = Program.ModuleEntries.Select(func).ToArray();
                Task.WhenAll(tasks).GetAwaiter().GetResult();
            }
            var ctx = new ReadyContext(app.ApplicationServices,Configuration);
            ctx.SetApplicationBuilder(app);
            ctx.SetHostEnvironment(env);
            ctx.SetWebRootPath(env.ContentRootPath);
            ctx.SetWebRootFileProvider(env.WebRootFileProvider);
            ctx.SetIsDevelopment(env.IsDevelopment());
            
            Run(x => x.BeforeReadyAsync(ctx));
            Run(x => x.ReadyAsync(ctx));
            Run(x => x.AfterReadyAsync(ctx));

#if DEBUG
            using (var scope = app.ApplicationServices.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<QnydDbContext>())
            {
                db.Database.EnsureCreated();
            }
#endif
        }
    }
}
