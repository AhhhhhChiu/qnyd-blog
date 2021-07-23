using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qnyd.Data
{
    public class QnydTag : QnydDbEntity
    {
        [Key]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        public int RefCount { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<QnydArticle> Articles { get; set; }
    }
}
