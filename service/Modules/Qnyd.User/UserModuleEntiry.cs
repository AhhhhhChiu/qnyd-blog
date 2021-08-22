using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Structing;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Qnyd.User
{
    public class UserModuleEntry : AutoModuleEntity
    {
        public override void Register(IRegisteContext context)
        {
            var builder = context.GetMvcBuilder();
            builder.AddApplicationPart(GetType().Assembly);
            context.Services.AddAuthentication();
            context.Services.AddAuthorization();
            context.Services.AddScoped<UserIdentityService>();
            context.Services.AddScoped<UserService>();
            context.Services.AddSingleton<IAuthorizationMiddlewareResultHandler,
                          QnydAuthorizationMiddlewareResultHandler>();
            base.Register(context);
        }
        public override Task ReadyAsync(IReadyContext context)
        {
            var builder = context.GetApplicationBuilder();
            builder.UseAuthentication();
            builder.UseAuthorization();

            return base.ReadyAsync(context);
        }
    }
}
