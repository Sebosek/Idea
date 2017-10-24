using Idea.Sample.Internals.Entities;

using Microsoft.EntityFrameworkCore;

namespace Idea.Sample.Internals.DbContext
{
    public class SampleDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SampleDbContext()
        {
        }

        public SampleDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>(pt =>
            {
                pt.HasKey(k => k.Id);
                pt.HasIndex(i => new
                {
                    i.PostId,
                    i.TagId,
                    i.Canceled
                });
                pt.HasOne(o => o.Post).WithMany(m => m.PostTags).HasForeignKey(k => k.PostId);
                pt.HasOne(o => o.Tag).WithMany(m => m.PostTags).HasForeignKey(k => k.TagId);
                pt.HasQueryFilter(f => f.Canceled == null);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(key => key.Id);
                post.HasIndex(i => i.Canceled);
                post.Property(p => p.Title).IsRequired().HasMaxLength(128);
                post.Property(p => p.Content).IsRequired();
                post.HasQueryFilter(f => f.Canceled == null);
            });

            modelBuilder.Entity<Tag>(tag =>
            {
                tag.HasKey(key => key.Id);
                tag.HasIndex(i => i.Canceled);
                tag.Property(p => p.Value).IsRequired().HasMaxLength(128);
                tag.HasQueryFilter(f => f.Canceled == null);
            });
        }
    }
}