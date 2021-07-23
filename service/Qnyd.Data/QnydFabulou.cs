using System.ComponentModel.DataAnnotations;

namespace Qnyd.Data
{
    public class QnydFabulou : QnydDbEntity
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long ArticleId { get; set; }

        public virtual QnydArticle Article { get; set; }

        public virtual QnydUser User { get; set; }
    }
}
