using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qnyd.Article.Rp
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }

        public string[] Tags { get; set; }

        public bool Check()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
