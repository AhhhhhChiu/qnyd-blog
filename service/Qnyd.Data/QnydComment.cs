using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qnyd.Data
{
    public class QnydComment : QnydIdentityDbEntity
    {        
        public string Content { get; set; }

        [Required]
        [ForeignKey(nameof(Article))]
        public long ArticleId { get; set; }

        [ForeignKey(nameof(PrevComment))]
        public long? PrevCommentId { get; set; }

        public QnydArticle Article { get; set; }

        public QnydComment PrevComment { get; set; }
    }
}
