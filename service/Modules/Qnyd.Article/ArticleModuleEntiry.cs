using Microsoft.Extensions.DependencyInjection;
using Structing;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.Article
{
    public class ArticleModuleEntiry : AutoModuleEntity
    {
        public override void Register(IRegisteContext context)
        {
            var builder = context.GetMvcBuilder();
            builder.AddApplicationPart(GetType().Assembly);
            base.Register(context);
        }
    }
}
