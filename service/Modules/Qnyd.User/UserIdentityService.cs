using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Qnyd.User
{
    public class UserIdentityService
    {
        private static readonly string UserMapKey = "Anf.Web.Services.UserIdentityService.UserMap";
        private readonly IMemoryCache database;

        public static readonly TimeSpan ExpireTime = TimeSpan.FromDays(6);

        public UserIdentityService(IMemoryCache database)
        {
            this.database = database;
        }

        public UserSnapshot GetTokenInfo(string token)
        {
            var key = RedisKeyGenerator.Concat(UserMapKey, token);
            var val = database.Get<UserSnapshot>(key);
            return val;
        }
        public string SetIdentity(UserSnapshot snapshot)
        {
            var tk = Guid.NewGuid().ToString();
            var key = RedisKeyGenerator.Concat(UserMapKey, tk);
            var bytes = JsonSerializer.SerializeToUtf8Bytes(snapshot);
            database.Set(key, bytes, ExpireTime);
            return tk;
        }
    }
}
