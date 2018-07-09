using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;
using Idea.Cookbook.Services;
using Idea.Repository;
using Idea.UnitOfWork;
using Idea.UnitOfWork.EntityFrameworkCore;

using Moq;

using Xunit;

namespace Idea.Cookbook.Tests.Services
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Unit tests name convetion")]
    public class UnitServiceTests
    {
        private readonly Mock<IRepository<Unit, Guid>> _repository;

        private readonly Mock<IUnitOfWorkFactory> _factory;

        private readonly Mock<IDataProvider> _provider;

        private readonly Mock<IUnitOfWork> _uow;

        public UnitServiceTests()
        {
            _repository = new Mock<IRepository<Unit, Guid>>();
            _factory = new Mock<IUnitOfWorkFactory>();
            _provider = new Mock<IDataProvider>();
            _uow = new Mock<IUnitOfWork>();

            _factory.Setup(s => s.Create()).Returns(_uow.Object);
            _factory.Setup(s => s.DataProvider()).Returns(_provider.Object);
        }

        [Fact]
        public async Task CreateUnitAsync_ValidData_ShouldSuccess()
        {
            var unit = new Unit();
            var service = new UnitService(_repository.Object, _factory.Object);

            var result = await service.CreateUnitAsync(unit);

            _repository.Verify(a => a.CreateAsync(unit));
            _uow.Verify(a => a.CommitAsync());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateUnitAsync_ValidData_ShouldSuccess()
        {
            var before = new Unit { Name = "before", Symbol = "before" };
            var after = new Unit { Name = "after", Symbol = "after" };
            var service = new UnitService(_repository.Object, _factory.Object);
            _repository.Setup(s => s.FindAsync(It.IsAny<Guid>())).ReturnsAsync(before);

            var result = await service.UpdateUnitAsync(Guid.Empty, after);

            _repository.Verify(a => a.UpdateAsync(It.IsAny<Unit>()));
            _uow.Verify(a => a.CommitAsync());
            Assert.Equal("after", result.Name);
            Assert.Equal("after", result.Symbol);
        }

        [Fact]
        public async Task RemoveEntityAsync_ValidData_ShouldSuccess()
        {
            var service = new UnitService(_repository.Object, _factory.Object);
            _repository.Setup(s => s.FindAsync(It.IsAny<Guid>())).ReturnsAsync(new Unit());

            await service.RemoveUnitAsync(Guid.Empty);

            _repository.Verify(a => a.DeleteAsync(It.IsAny<Unit>()));
            _uow.Verify(a => a.CommitAsync());
        }

        [Fact]
        public async Task GetUnitsAsync_ValidData_ShouldSuccess()
        {
            var units = new[] { new Unit() };
            var service = new UnitService(_repository.Object, _factory.Object);
            _provider.Setup(s => s.Data<Unit, Guid>()).Returns(units.AsQueryable());

            var result = await service.GetUnitsAsync();

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetUnitAsync_ValidData_ShouldSuccess()
        {
            var service = new UnitService(_repository.Object, _factory.Object);
            _repository.Setup(s => s.FindAsync(It.IsAny<Guid>())).ReturnsAsync(new Unit());

            var result = await service.GetUnitAsync(Guid.Empty);

            Assert.NotNull(result);
        }
    }
}