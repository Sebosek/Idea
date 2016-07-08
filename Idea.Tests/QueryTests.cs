//using System;
//using System.Linq;
//using System.Linq.Expressions;
//using Idea.Tests.Entity;
//using Idea.Tests.Fixtures;
//using Idea.Tests.Fixtures.Seed;
//using Idea.UnitOfWork;
//using Idea.UnitOfWork.EntityFrameworkCore;
//using Xunit;

//namespace Idea.Tests
//{
//    [Collection("Database collection")]
//    public class QueryTests
//    {
//        private int BookCount = BookSeed.BOOKS_DATA.Length;

//        private readonly DbFixture _fixture;
//        private readonly IUnitOfWorkFactory _factory;

//        public QueryTests(DbFixture fixture)
//        {
//            _fixture = fixture;
//            _factory = new UnitOfWorkFactory(_fixture.DbContextFactory, _fixture.UowManager);
//        }

//        [Fact]
//        public void Skip_ZeroValue_ShouldSuccess()
//        {
//            const int skip = 0;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.Skip = skip;

//                var data = query.Execute(uow);

//                Assert.Equal(BookCount, data.Count);
//            }
//        }

//        [Fact]
//        public void Skip_PositiveValue_ShouldSuccess()
//        {
//            const int skip = 10;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.Skip = skip;

//                var data = query.Execute(uow);

//                Assert.Equal(BookCount - skip, data.Count);
//            }
//        }

//        [Fact]
//        public void Skip_NegativeValue_ShouldSuccess()
//        {
//            const int skip = -10;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.Skip = skip;

//                var data = query.Execute(uow);

//                Assert.Equal(BookCount, data.Count);
//            }
//        }

//        [Fact]
//        public void Take_ZeroValue_ShouldSuccess()
//        {
//            const int take = 0;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.Take = take;

//                var data = query.Execute(uow);

//                Assert.Equal(0, data.Count);
//            }
//        }

//        [Fact]
//        public void Take_PositiveValue_ShouldSuccess()
//        {
//            const int take = 10;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.Take = take;

//                var data = query.Execute(uow);

//                Assert.Equal(take, data.Count);
//            }
//        }

//        [Fact]
//        public void Take_NegativeValue_ShouldSuccess()
//        {
//            const int take = -10;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.Take = take;

//                var data = query.Execute(uow);

//                Assert.Equal(0, data.Count);
//            }
//        }

//        [Fact]
//        public void AddOrderBy_ValidStringPropertyAsc_ShouldSuccess()
//        {
//            const string property = "Title";

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.AddOrderBy(property, Order.Asc);

//                var data = query.Execute(uow);

//                var ordered = BookSeed.BOOKS_DATA.OrderBy(o => o.Title);
//                Assert.Equal(ordered.First().Title, data.First().Title);
//                Assert.Equal(ordered.Last().Title, data.Last().Title);
//            }
//        }

//        [Fact]
//        public void AddOrderBy_ValidStringPropertyDesc_ShouldSuccess()
//        {
//            const string property = "Title";

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.AddOrderBy(property, Order.Desc);

//                var data = query.Execute(uow);

//                var ordered = BookSeed.BOOKS_DATA.OrderByDescending(o => o.Title);
//                Assert.Equal(ordered.First().Title, data.First().Title);
//                Assert.Equal(ordered.Last().Title, data.Last().Title);
//            }
//        }

//        [Fact]
//        public void AddOrderBy_EmptyStringProperty_ShouldThrowException()
//        {
//            const string property = "";

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();

//                Assert.Throws<ShBadInputException>(() => query.AddOrderBy(property, Order.Desc));
//            }
//        }

//        [Fact]
//        public void AddOrderBy_NullProperty_ShouldThrowException()
//        {
//            const string property = null;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();

//                Assert.Throws<ShBadInputException>(() => query.AddOrderBy(property, Order.Desc));
//            }
//        }

//        [Fact]
//        public void AddOrderBy_StringWithNonUsableCharactersForPropertyProperty_ShouldThrowException()
//        {
//            const string property = "%*";

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();

//                Assert.Throws<ShBadInputException>(() => query.AddOrderBy(property, Order.Desc));
//            }
//        }

//        [Fact]
//        public void AddOrderBy_ValidExpressionAsc_ShouldSuccess()
//        {
//            Expression<Func<Book, object>> expr = e => e.Title;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.AddOrderBy(expr, Order.Asc);

//                var data = query.Execute(uow);

//                var ordered = BookSeed.BOOKS_DATA.OrderBy(o => o.Title);
//                Assert.Equal(ordered.First().Title, data.First().Title);
//                Assert.Equal(ordered.Last().Title, data.Last().Title);
//            }
//        }

//        [Fact]
//        public void AddOrderBy_ValidExpressionDesc_ShouldSuccess()
//        {
//            Expression<Func<Book, object>> expr = e => e.Title;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.AddOrderBy(expr, Order.Desc);

//                var data = query.Execute(uow);

//                var ordered = BookSeed.BOOKS_DATA.OrderByDescending(o => o.Title);
//                Assert.Equal(ordered.First().Title, data.First().Title);
//                Assert.Equal(ordered.Last().Title, data.Last().Title);
//            }
//        }

//        [Fact]
//        public void AddOrderBy_ExpressionWithNull_ShouldSuccess()
//        {
//            Expression<Func<Book, object>> expr = e => null;

//            using (var uow = _factory.Create())
//            {
//                var query = new Stumbs.Query();
//                query.AddOrderBy(expr, Order.Desc);

//                var data = query.Execute(uow);
                
//                Assert.Equal(BookCount, data.Count);
//            }
//        }
//    }
//}
