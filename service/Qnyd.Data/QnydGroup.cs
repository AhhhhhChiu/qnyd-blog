using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Qnyd.Data
{
    public class QnydGroup : QnydIdentityDbEntity
    {


        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }

        public virtual ICollection<QnydArticle> Articles { get; set; }

        public virtual QnydUser User { get; set; }
    }
}
