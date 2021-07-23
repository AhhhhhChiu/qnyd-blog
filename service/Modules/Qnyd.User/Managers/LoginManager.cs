using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Qnyd.User.Managers
{
    [EnableService(ServiceLifetime = ServiceLifetime.Singleton,
        ServiceType =typeof(ILoginManager))]
    internal class LoginManager : ILoginManager
    {
        private static readonly string TokenCacheHeader = "Qnyd.User.Managers.LoginManager.UserToken.";
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;

        public LoginManager(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
        }

        private TimeSpan GetTokenAliveTime()
        {
            return configuration.GetValue<TimeSpan>("User:Token:AliveTime");
        }
        public string SetAccessToken(string userName)
        {
            var tk = Guid.NewGuid().ToString();
            SetAccessToken(userName, tk);
            return tk;
        }
        public void SetAccessToken(string userName, string token)
        {
            var time = GetTokenAliveTime();
            memoryCache.Set(TokenCacheHeader + userName, token, time);
        }
        public bool HasToken(string userName)
        {
            var key = TokenCacheHeader + userName;
            return memoryCache.Get(key) != null;
        }
        public bool IsTokenEqual(string userName, string comparerToken)
        {
            var key = TokenCacheHeader + userName;
            var tk = memoryCache.Get(key);
            return tk != null && tk.ToString() == comparerToken;
        }
    }
}
