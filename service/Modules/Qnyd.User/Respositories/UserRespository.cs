using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Qnyd.Data;
using Qnyd.User.Managers;
using Structing.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.User.Respositories
{
    [EnableService(ServiceLifetime = ServiceLifetime.Scoped)]
    public class UserRespository
    {
        private readonly ILoginManager loginManager;
        private readonly UserManager<QnydUser> userManager;

        public UserRespository(ILoginManager loginManager, 
            UserManager<QnydUser> userManager)
        {
            this.loginManager = loginManager;
            this.userManager = userManager;
        }
        public async Task<string> LoginAsync(string userName,string pwd)
        {
            var user =await userManager.FindByNameAsync(userName);
            if (user is null)
            {
                return null;
            }
            var succeed =await userManager.CheckPasswordAsync(user, pwd);
            if (succeed)
            {
                return loginManager.SetAccessToken(userName);
            }
            return null;
        }

        public async Task<bool> RegistAsync(string userName,string pwd)
        {
            var user = new QnydUser
            {
                UserName = userName,
                NormalizedUserName = userName
            };
            var result = await userManager.CreateAsync(user, pwd);
            return result.Succeeded;
        }
    }
}
