using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Qnyd.Data;
using Qnyd.Data.Results;
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
        private readonly UserRespository userRespository;

        public UserController(UserRespository userRespository)
        {
            this.userRespository = userRespository;
        }
        [HttpPost]
        [ProducesResponseType(typeof(EntityResult<string>),200)]
        public async Task<IActionResult> Login([FromForm]string userName,[FromForm]string pwd)
        {
            var tk =await userRespository.LoginAsync(userName, pwd);
            var result = new EntityResult<string> { Entity = tk, Succeed = tk != null };
            return Ok(result);
        }
        [HttpPost]
        [ProducesResponseType(typeof(EntityResult<bool>), 200)]
        public async Task<IActionResult> Regist([FromForm]string userName, [FromForm]string pwd)
        {
            var succeed = await userRespository.RegistAsync(userName, pwd);
            var result = new EntityResult<bool> { Entity = succeed };
            return Ok(result);
        }
    }
}
