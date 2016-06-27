using System.Collections.Generic;
using Idea.Entity;

namespace Idea.Repository.EntityFrameworkCore.Tests.Mocks
{
    public class Hero : Entity<string>
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Origin { get; set; }

        public IList<HeroRelationship> Relationships { get; set; }

        public Hero()
        {
            Relationships = new List<HeroRelationship>();
        }
    }
}
