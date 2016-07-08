using Idea.Repository;
using Idea.Tests.Entity;

namespace Idea.Tests.Repository
{
    interface IAuthorRepository : IRepository<Author, int>
    { }
}
