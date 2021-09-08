using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.Article.Models
{
    public class ArticleGroup
    {
        [BsonId]
        public BsonObjectId Id { get; set; }

        public string Name { get; set; }

        public string[] Tags { get; set; }

        public DateTime CreateTime { get; set; }
    }
    public class ArticlePager
    {
        [BsonId]
        public BsonObjectId Id { get; set; }

        public BsonObjectId PaperId { get; set; }

        public string[] Tags { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
