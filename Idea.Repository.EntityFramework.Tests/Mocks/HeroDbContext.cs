using Microsoft.EntityFrameworkCore;

namespace Idea7.Repository.EntityFramework.Tests.Mocks
{
    public class HeroDbContext : DbContext
    {
        public DbSet<Hero> Heroes { get; set; }

        public HeroDbContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hero>()
                .Property(p => p.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<HeroRelationship>()
                .HasOne(pt => pt.Hero)
                .WithMany(p => p.Relationships);

            modelBuilder.Entity<HeroRelationship>()
                .HasOne(pt => pt.ToHero)
                .WithMany(t => t.Relationships);
        }
    }
}
