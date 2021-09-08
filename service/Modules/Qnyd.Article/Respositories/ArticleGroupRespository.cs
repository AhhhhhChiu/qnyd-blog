using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Qnyd.Article.Models;
using Qnyd.Article.Rp;
using Qnyd.Data.Results;
using Structing.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.Article.Respositories
{
    [EnableService(ServiceLifetime = ServiceLifetime.Scoped)]
    public class ArticleGroupRespository
    {
        private readonly IMongoClient mongoClient;
        private readonly ILogger<ArticleGroupRespository> logger;

        public ArticleGroupRespository(IMongoClient mongoClient, ILogger<ArticleGroupRespository> logger)
        {
            this.mongoClient = mongoClient;
            this.logger = logger;
        }

        public async Task<EntityRangeResult<List<ArticleItem>>> GetAsync(int? skip, int? take)
        {
            var coll = mongoClient.GetGroupCollection();
            var filter = Builders<ArticleGroup>.Filter.Empty;
            var query = coll.Find(filter);
            var count = await query.CountDocumentsAsync();
            if (skip != null)
            {
                query = query.Skip(skip);
            }
            if (take != null)
            {
                query = query.Limit(take);
            }
            var datas = await query.SortByDescending(x => x.CreateTime)
                .Project(x => new ArticleItem
                {
                    Id = x.Id.ToString(),
                    CreateTime = x.CreateTime,
                    Name = x.Name,
                    Tags = x.Tags
                })
                .ToListAsync();
            var res = new EntityRangeResult<List<ArticleItem>>
            {
                Entity = datas,
                Skip = skip,
                Take = take,
                Total = count,
                Succeed = true
            };
            return res;
        }
        public Task<UpdateResult> UpdateNameAsync(BsonObjectId id,string name)
        {
            var coll = mongoClient.GetGroupCollection();
            var filter = Builders<ArticleGroup>.Filter.Eq(x => x.Id, id);
            var update = Builders<ArticleGroup>.Update.Set(x => x.Name, name);
            logger.LogInformation("Will update {0} article name to {1}", id, name);

            return coll.UpdateOneAsync(filter, update);
        }
        public Task<DeleteResult> RemoveAsync(BsonObjectId id)
        {
            var coll = mongoClient.GetGroupCollection();
            var filter = Builders<ArticleGroup>.Filter.Eq(x => x.Id, id);
            logger.LogInformation("Will remove article group {0}", id);
            return coll.DeleteOneAsync(filter);
        }
        public Task CreateAsync(string name, string[] tags)
        {
            var coll = mongoClient.GetGroupCollection();
            var ds = new ArticleGroup
            {
                CreateTime = DateTime.Now,
                Name = name,
                Tags = tags
            };
            logger.LogInformation("Will insert group {0}", name);
            return coll.InsertOneAsync(ds);
        }
    }
}
