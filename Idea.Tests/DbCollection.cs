using Idea.Tests.Fixture;
using Xunit;

namespace Idea.Tests
{
    [CollectionDefinition("Database collection")]
    public class DbCollection : ICollectionFixture<DbFixture>
    { }
}
