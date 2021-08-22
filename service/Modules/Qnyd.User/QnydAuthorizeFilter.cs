using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Qnyd.User.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Qnyd.User
{
    internal class QnydAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        internal const string TokenKey = "qnyd";
        public Task HandleAsync(RequestDelegate next, 
            HttpContext context, 
            AuthorizationPolicy policy, 
            PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Challenged)
            {
                var ser=context.RequestServices.GetRequiredService<UserIdentityService>();
                string token = null;
                if (context.Request.Query.TryGetValue(TokenKey, out var qv) &&
                    qv.Count > 0)
                {
                    token = qv[0];
                }
                else if (context.Request.Cookies != null)
                {
                    context.Request.Cookies.TryGetValue(TokenKey, out token);
                }
                if (token == null)
                {
                    var tokenInfo = ser.GetTokenInfo(token);
                    if (tokenInfo is null)
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                    context.Features.Set(tokenInfo);
                }
            }
            return next(context);
        }
    }
}
