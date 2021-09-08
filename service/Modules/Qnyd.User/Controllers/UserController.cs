using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Qnyd.Data;
using Qnyd.Data.Results;
using Qnyd.User.Helpers;
using Qnyd.User.Respositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.User.Controllers
{
    [Route(QnydConst.RouteWithControllerAction)]
    public class QnydController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult A()
        {
            return Ok("a");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult B()
        {
            return Ok("b");
        }
    }
    [Route(QnydConst.RouteWithControllerAction)]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(EntityResult<RSAKeyIdentity>), 200)]
        public IActionResult FlushKey()
        {
            var key = userService.GetSharedRSAKey();
            var res = new EntityResult<RSAKeyIdentity> { Entity = key };
            return Ok(res);
        }
        [HttpGet]
        [ProducesResponseType(typeof(EntityResult<bool>), 200)]
        public IActionResult IsLogin([FromQuery]string token)
        {
            var ok=userService.IsLogin(token);
            var res = new EntityResult<bool> { Entity = ok };
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(EntityResult<string>), 200)]
        public async Task<IActionResult> Login([FromForm] string userName, [FromForm] string passwordHash, [FromForm] string connectId)
        {
            var tk = await userService.LoginAsync(connectId, userName, passwordHash);
            if (!string.IsNullOrEmpty(tk))
            {
                HttpContext.Response.Cookies.Append(QnydAuthorizationMiddlewareResultHandler.TokenKey, tk, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    MaxAge = UserIdentityService.ExpireTime
                });
            }
            var res = new EntityResult<string> { Entity = tk };
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(EntityResult<bool>), 200)]
        public async Task<IActionResult> Registe([FromForm] string userName, [FromForm] string passwordHash, [FromForm] string connectId)
        {
            var succeed = await userService.RegisteAsync(connectId, userName, passwordHash);
            if (!succeed)
            {
                return StatusCode((int)StatusCodes.Status406NotAcceptable);
            }
            var res = new EntityResult<bool> { Entity = succeed };
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(EntityResult<bool>), 200)]
        public async Task<IActionResult> ResetPwd(string userName, string tk, string pwd)
        {
            var resetRes = await userService.RestPasswordAsync(HttpContext.Session.Id, userName, tk, pwd);
            var res = new EntityResult<bool> { Entity = resetRes };
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(EntityResult<bool>), 200)]
        public async Task<IActionResult> ResetPwdWithOld(string userName, string old, string pwd)
        {
            var resetRes = await userService.RestPasswordWithOldAsync(HttpContext.Session.Id, userName, old, pwd);
            var res = new EntityResult<bool> { Entity = resetRes };
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(EntityResult<string>), 200)]
        public async Task<IActionResult> GenerateResetToken(string userName)
        {
            //TODO:发邮件
            var tk = await userService.GenerateResetTokenAsync(userName);
            var res = new EntityResult<string> { Entity = tk };
            return Ok(res);
        }
    }
}
