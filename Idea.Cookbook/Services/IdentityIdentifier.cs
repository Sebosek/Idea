using System;

using Idea.UnitOfWork;

namespace Idea.Cookbook.Services
{
    public class IdentityIdentifier : IIdentityIdentifier<Guid>
    {
        public Guid IdentityKey() => Guid.NewGuid();
    }
}