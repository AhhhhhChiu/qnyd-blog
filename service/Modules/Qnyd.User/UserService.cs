using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Qnyd.Data;
using Qnyd.User.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.User
{
    public class UserService
    {
        private static readonly string RSAKey = "Qnyd.Web.Services.UserService.RSAKey";
        private static readonly string CurrentIdentityKey = "Qnyd.Web.Services.UserService.CurrentIdentity";
        private static readonly TimeSpan RSAKeyCacheTime = TimeSpan.FromMinutes(3);
        private static readonly TimeSpan CurrentRSAKeyCacheTime = TimeSpan.FromDays(1);

        private readonly IMemoryCache database;
        private readonly UserIdentityService userIdentityService;
        private readonly UserManager<QnydUser> userManager;

        public UserService(IMemoryCache database,
            UserIdentityService userIdentityService,
            UserManager<QnydUser> userManager)
        {
            this.userIdentityService = userIdentityService;
            this.database = database;
            this.userManager = userManager;
        }

        public RSAKeyIdentity GetSharedRSAKey()
        {
            var dent = database.Get<RSAKeyIdentity>(CurrentIdentityKey);
            if (dent is null)
            {
                dent = FlushRSAKey(CurrentRSAKeyCacheTime);
                database.Set(CurrentIdentityKey, dent, CurrentRSAKeyCacheTime);
            }
            return dent;
        }
        public RSAKeyIdentity FlushRSAKey()
        {
            return FlushRSAKey(RSAKeyCacheTime);
        }
        public RSAKeyIdentity FlushRSAKey(TimeSpan cacheTime)
        {
            var rsaKey = RSAHelper.GenerateRSASecretKey();
            var identity = Guid.NewGuid().ToString();
            var key = RedisKeyGenerator.Concat(RSAKey,identity);
            database.Set(key, rsaKey.PrivateKey, cacheTime);
            return new RSAKeyIdentity
            {
                Key = rsaKey.PublicKey,
                Identity = identity
            };
        }
        public bool IsLogin(string token)
        {
            return userIdentityService.GetTokenInfo(token) != null;
        }
        public Task<string> GenerateResetTokenAsync(string userName)
        {
            var user = new QnydUser { UserName = userName };
            return userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<bool> RestPasswordAsync(string connectId, string userName, string resetToken, string @new)
        {
            var privateKey = GetPrivateKeyAsync(connectId);
            if (privateKey is null)
            {
                return false;
            }
            var pwdNew = RSAHelper.RSADecrypt(privateKey, @new);
            if (string.IsNullOrEmpty(pwdNew))
            {
                return false;
            }
            var user = new QnydUser { UserName = userName };
            var ok = await userManager.ResetPasswordAsync(user, resetToken, pwdNew);
            return ok.Succeeded;
        }
        public async Task<bool> RestPasswordWithOldAsync(string connectId, string userName, string old, string @new)
        {
            var privateKey = GetPrivateKeyAsync(connectId);
            if (privateKey is null)
            {
                return false;
            }
            var pwdOld = RSAHelper.RSADecrypt(privateKey, old);
            if (string.IsNullOrEmpty(pwdOld))
            {
                return false;
            }
            var pwdNew = RSAHelper.RSADecrypt(privateKey, @new);
            if (string.IsNullOrEmpty(pwdNew))
            {
                return false;
            }
            var user = new QnydUser { UserName = userName };
            var ok = await userManager.ChangePasswordAsync(user, pwdOld, pwdNew);
            return ok.Succeeded;
        }
        public async Task<bool> RegisteAsync(string connectId, string userName, string passwordHash)
        {
            var privateKey = GetPrivateKeyAsync(connectId);
            if (privateKey is null)
            {
                return false;
            }
            var pwd = RSAHelper.RSADecrypt(privateKey, passwordHash);
            if (string.IsNullOrEmpty(pwd))
            {
                return false;
            }
            var user = new QnydUser { UserName = userName };
            var identity = await userManager.CreateAsync(user, pwd);
            return identity.Succeeded;
        }
        private string GetPrivateKeyAsync(string connectId)
        {
            var key = RedisKeyGenerator.Concat(RSAKey, connectId);
            return database.Get<string>(key);
        }
        public async Task<string> LoginAsync(string connectId, string userName, string passwordHash)
        {
            var privateKey = GetPrivateKeyAsync(connectId);
            if (privateKey is null)
            {
                return null;
            }
            var val = RSAHelper.RSADecrypt(privateKey, passwordHash);
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }
            var u = await userManager.FindByNameAsync(userName);
            var ok = await userManager.CheckPasswordAsync(u, val);
            if (ok)
            {
                var key = RedisKeyGenerator.Concat(RSAKey, connectId);
                database.Remove(key);
                var identity = new UserSnapshot
                {
                    Email = u.Email,
                    Id = u.Id,
                    Name = u.NormalizedUserName
                };
                var tk = userIdentityService.SetIdentity(identity);
                return tk;
            }
            return null;
        }
    }
}
