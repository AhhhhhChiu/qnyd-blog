using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qnyd.Data
{
    public class QnydUser : IdentityUser<long>
    {
        public virtual ICollection<QnydArticle> Articles { get; set; }

        public virtual ICollection<QnydGroup> Groups { get; set; }
    }
}
