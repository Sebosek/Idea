﻿using System.Collections.Generic;
using Idea7.Entity;

namespace Idea7.Repository.EntityFramework.Tests.Mocks
{
    public class Hero : Entity<string>
    {
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Origin { get; set; }
        public IEnumerable<Hero> Friends { get; set; }
        public IEnumerable<Hero> Enemies { get; set; }
    }
}
