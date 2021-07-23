using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Qnyd.Data
{
    public class QnydDbContext : IdentityDbContext<QnydUser, QnydRole, long>
    {
        public QnydDbContext(DbContextOptions options) : base(options)
        {
        }

        protected QnydDbContext()
        {
        }

        public virtual DbSet<QnydGroup> Groups => Set<QnydGroup>();
        public virtual DbSet<QnydTag> Tags => Set<QnydTag>();
        public virtual DbSet<QnydArticle> Articles => Set<QnydArticle>();
        public virtual DbSet<QnydComment> Comments => Set<QnydComment>();
        public virtual DbSet<QnydFabulou> Fabulous => Set<QnydFabulou>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<QnydFabulou>(b =>
            {
                b.HasKey(nameof(QnydFabulou.ArticleId), nameof(QnydFabulou.UserId));
                b.HasQueryFilter(x => x.Enable);
            });
            builder.Entity<QnydTag>(b =>
            {
                b.HasMany(x => x.Articles).WithMany(x => x.Tags);
                b.HasQueryFilter(x => x.Enable);
            });
            builder.Entity<QnydGroup>(b =>
            {
                b.HasQueryFilter(x => x.Enable);
            });
            builder.Entity<QnydComment>(b =>
            {
                b.HasQueryFilter(x=>x.Enable);
            });
            builder.Entity<QnydGroup>(b =>
            {
                b.HasQueryFilter(x => x.Enable);
            });
        }
    }
}
