using Idea7.Entity;

namespace Idea7.Repository.EntityFramework.Tests.Mocks
{
    public class HeroRelationship : Entity<string>
    {
        public Hero Hero { get; set; }
        public Hero ToHero { get; set; }
        public Relationship Relationship { get; set; }
    }
}
