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

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(blog =>
            {
                blog.HasKey(key => key.Id);
                blog.HasIndex(i => i.Created);
                blog.HasIndex(i => i.Canceled);
                blog.Property(p => p.Name).IsRequired().HasMaxLength(128);
                blog.Property(p => p.Owner).IsRequired().HasMaxLength(128);
                blog.HasQueryFilter(f => f.Canceled == null);
            });

            modelBuilder.Entity<PostTag>(pt =>
            {
                pt.HasKey(k => new 
                {
                    k.PostId, k.TagId
                });
                pt.HasOne(o => o.Post).WithMany(m => m.PostTags).HasForeignKey(k => k.PostId);
                pt.HasOne(o => o.Tag).WithMany(m => m.PostTags).HasForeignKey(k => k.TagId);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(key => key.Id);
                post.HasIndex(i => i.Created);
                post.HasIndex(i => i.Canceled);
                post.Property(p => p.Title).IsRequired().HasMaxLength(128);
                post.Property(p => p.Content).IsRequired();
                post.HasQueryFilter(f => f.Canceled == null);
            });

            modelBuilder.Entity<Tag>(tag =>
            {
                tag.HasKey(key => key.Id);
                tag.HasIndex(i => i.Created);
                tag.HasIndex(i => i.Canceled);
                tag.Property(p => p.Value).IsRequired().HasMaxLength(128);
                tag.HasQueryFilter(f => f.Canceled == null);
            });
        }
    }
}