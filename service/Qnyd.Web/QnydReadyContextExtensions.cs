using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace Structing.Core
{
    internal static class QnydReadyContextExtensions
    {
        const string WebRootPathKey = "WebRootPath";
        const string IsDevelopmentKey = "IsDevelopment";
        
        public static IWebHostBuilder GetWebHostBuilder(this IServiceProvider provider)
        {
            return provider.GetService<IWebHostBuilder>();
        }
        public static void SetValue(this IRegisteContext context, object name, object value)
        {
            context.Features[name] = value;
        }

        public static void SetValue(this IReadyContext context,object name, object value)
        {
            context.Features[name] = value;
        }
        public static void SetValue<T>(this IRegisteContext context, T value)
        {
            SetValue(context, typeof(T).Name, value);
        }

        public static void SetValue<T>(this IReadyContext context,T value)
        {
            SetValue(context, typeof(T).Name, value);
        }
        public static object GetValue(this IRegisteContext context, object name)
        {
            if (context.Features.Contains(name))
            {
                return context.Features[name];
            }
            return null;
        }

        public static object GetValue(this IReadyContext context,object name)
        {
            if (context.Features.Contains(name))
            {
                return context.Features[name];
            }
            return null;
        }
        public static T GetValue<T>(this IRegisteContext context)
        {
            var name = typeof(T).Name;
            var val = GetValue(context, name);
            if (val == null)
            {
                return default(T);
            }
            return (T)val;
        }

        public static T GetValue<T>(this IReadyContext context)
        {
            var name = typeof(T).Name;
            var val = GetValue(context, name);
            if (val ==null)
            {
                return default(T);
            }
            return (T)val;
        }
        public static void SetConfiguration(this IRegisteContext context, IConfiguration configuration)
        {
            SetValue(context, configuration);
        }

        public static IConfiguration GetConfiguration(this IRegisteContext context)
        {
            return GetValue<IConfiguration>(context);
        }

        public static void SetMvcBuilder(this IRegisteContext context, IMvcBuilder mvcBuilder)
        {
            SetValue(context, mvcBuilder);
        }
        public static IMvcBuilder GetMvcBuilder(this IRegisteContext context)
        {
            return GetValue<IMvcBuilder>(context);
        }
        public static void SetApplicationBuilder(this IReadyContext context,IApplicationBuilder builder)
        {
            SetValue(context, builder);
        }
        public static IApplicationBuilder GetApplicationBuilder(this IReadyContext context)
        {
            return GetValue<IApplicationBuilder>(context);
        }
        public static void SetHostEnvironment(this IReadyContext context, IHostEnvironment host)
        {
            SetValue(context, host);
        }
        public static IHostEnvironment GetHostEnvironment(this IReadyContext context)
        {
            return GetValue<IHostEnvironment>(context);
        }
        public static void SetWebRootPath(this IReadyContext context, string webRootPath)
        {
            SetValue(context, WebRootPathKey, webRootPath);
        }
        public static string GetWebRootPath(this IReadyContext context)
        {
            return (string)GetValue(context, WebRootPathKey);
        }
        public static void SetWebRootFileProvider(this IReadyContext context, IFileProvider provider)
        {
            SetValue(context, provider);
        }
        public static IFileProvider GetWebRootFileProvider(this IReadyContext context)
        {
            return GetValue<IFileProvider>(context);
        }
        public static void SetIsDevelopment(this IReadyContext context, bool isDevelopment)
        {
            SetValue(context, IsDevelopmentKey, isDevelopment);
        }
        public static bool? GetIsDevelopment(this IReadyContext context)
        {
            return (bool?)GetValue(context, IsDevelopmentKey);
        }

    }
}
