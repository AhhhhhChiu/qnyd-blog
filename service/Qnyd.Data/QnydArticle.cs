using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qnyd.Data
{
    public class QnydArticle : QnydIdentityDbEntity
    {
        public DateTime? UpdateTime { get; set; }

        [Required]
        [MaxLength(128)]
        public string Title { get; set; }

        [Required]
        [MaxLength(128)]
        public string PhysicalFileName { get; set; }

        [ForeignKey(nameof(Group))]
        public long GroupId { get; set; }

        [Required]
        public long Fabulous { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }

        public virtual QnydGroup Group { get; set; }

        public virtual ICollection<QnydTag> Tags { get; set; }

        public QnydUser User { get; set; }
    }
}
