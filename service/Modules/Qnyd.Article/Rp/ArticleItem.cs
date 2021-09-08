using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.Article.Rp
{
    public class ArticleItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Tags { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
