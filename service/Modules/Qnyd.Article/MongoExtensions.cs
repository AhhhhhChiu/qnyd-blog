using MongoDB.Driver;
using Qnyd.Article.Models;
using Qnyd.Article.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.Article
{
    internal static class MongoExtensions
    {
        public static IMongoCollection<ArticleGroup> GetGroupCollection(this IMongoClient client)
        {
            var db = client.GetDatabase(ArticleMongoKeys.DbName);
            return db.GetCollection<ArticleGroup>(ArticleMongoKeys.GroupCollectionName);
        }
        public static IMongoCollection<ArticlePager> GetPaperCollection(this IMongoClient client)
        {
            var db = client.GetDatabase(ArticleMongoKeys.DbName);
            return db.GetCollection<ArticlePager>(ArticleMongoKeys.PaperCollectionName);
        }
    }
}
