using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Qnyd.Article.Respositories;
using Qnyd.Article.Rp;
using Qnyd.Data.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.Article.Controllers
{
    [Route(QnydConst.RouteWithControllerAction)]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleGroupRespository articleGroupRespository;

        public ArticleController(ArticleGroupRespository articleGroupRespository)
        {
            this.articleGroupRespository = articleGroupRespository;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> CreateGroup([FromBody]CreateGroupRequest request)
        {
            if (request is null||!request.Check())
            {
                return BadRequest();
            }
            await articleGroupRespository.CreateAsync(request.Name, request.Tags);
            return Ok(Result.CreateResult());
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(EntityRangeResult<List<ArticleItem>>), 200)]
        public async Task<IActionResult> GetGroup(int? skip,int? take)
        {
            var res=await articleGroupRespository.GetAsync(skip,take);
            return Ok(res);
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> RemoveGroup(string id)
        {
            if (!ObjectId.TryParse(id,out var oid))
            {
                return BadRequest("id");
            }
            var result=await articleGroupRespository.RemoveAsync(new BsonObjectId(oid));
            if (result.DeletedCount == 0)
            {
                return NotFound(id);
            }
            return Ok(Result.CreateResult());
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<IActionResult> UpdateName(string id,string name)
        {
            if (!ObjectId.TryParse(id, out var oid))
            {
                return BadRequest("id");
            }
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("name");
            }
            var result = await articleGroupRespository.UpdateNameAsync(new BsonObjectId(oid),name);
            if (result.MatchedCount == 0)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            return Ok(Result.CreateResult());
        }
    }
}
