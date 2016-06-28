using Idea.Entity;

namespace Idea.Repository.EntityFrameworkCore.Tests.Mocks
{
    public class HeroRelationship : Entity<string>
    {
        public Hero Hero { get; set; }
        public Hero ToHero { get; set; }
        public Relationship Relationship { get; set; }
    }
}
