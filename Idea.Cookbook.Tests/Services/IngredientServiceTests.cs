using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Idea.Cookbook.Entities;
using Idea.Cookbook.Services;
using Idea.Repository;
using Idea.UnitOfWork;

using Moq;

using Xunit;

namespace Idea.Cookbook.Tests.Services
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Unit tests name convetion")]
    public class IngredientServiceTests
    {
        private readonly Mock<IRepository<Ingredient, Guid>> _repository;

        private readonly Mock<IUnitOfWorkFactory> _factory;

        private readonly Mock<IDataProvider> _provider;

        private readonly Mock<IUnitOfWork> _uow;

        public IngredientServiceTests()
        {
            _repository = new Mock<IRepository<Ingredient, Guid>>();
            _factory = new Mock<IUnitOfWorkFactory>();
            _provider = new Mock<IDataProvider>();
            _uow = new Mock<IUnitOfWork>();

            _factory.Setup(s => s.Create()).Returns(_uow.Object);
            _factory.Setup(s => s.DataProvider()).Returns(_provider.Object);
        }

        [Fact]
        public async Task CreateIngredientAsync_ValidData_ShouldSuccess()
        {
            var ingredient = new Ingredient();
            var service = new IngredientService(_factory.Object, _repository.Object);

            var result = await service.CreateIngredientAsync(ingredient);

            _repository.Verify(a => a.CreateAsync(ingredient));
            _uow.Verify(a => a.CommitAsync());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateUnitAsync_ValidData_ShouldSuccess()
        {
            var before = new Ingredient { Name = "before", Amount = 0, Unit = new Unit()};
            var after = new Ingredient { Name = "after", Amount = 1 };
            var service = new IngredientService(_factory.Object, _repository.Object);
            _provider.Setup(s => s.Data<Ingredient, Guid>()).Returns(new[] { before }.AsQueryable());

            var result = await service.UpdateIngredientAsync(Guid.Empty, after);

            _repository.Verify(a => a.UpdateAsync(It.IsAny<Ingredient>()));
            _uow.Verify(a => a.CommitAsync());
            Assert.Equal("after", result.Name);
            Assert.Equal(1, result.Amount);
        }

        [Fact]
        public async Task RemoveIngredientAsync_ValidData_ShouldSuccess()
        {
            var service = new IngredientService(_factory.Object, _repository.Object);
            _repository.Setup(s => s.FindAsync(It.IsAny<Guid>())).ReturnsAsync(new Ingredient());

            await service.RemoveIngredientAsync(Guid.Empty);

            _repository.Verify(a => a.DeleteAsync(It.IsAny<Ingredient>()));
            _uow.Verify(a => a.CommitAsync());
        }

        [Fact]
        public async Task GetUnitsAsync_ValidData_ShouldSuccess()
        {
            var ingredients = new[] { new Ingredient() };
            var service = new IngredientService(_factory.Object, _repository.Object);
            _provider.Setup(s => s.Data<Ingredient, Guid>()).Returns(ingredients.AsQueryable());

            var result = await service.GetIngredientsAsync();

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetUnitAsync_ValidData_ShouldSuccess()
        {
            var id = Guid.NewGuid();
            var ingredients = new[] { new Ingredient { Id = id } };
            var service = new IngredientService(_factory.Object, _repository.Object);
            _provider.Setup(s => s.Data<Ingredient, Guid>()).Returns(ingredients.AsQueryable());

            var result = await service.GetIngredientAsync(id);

            Assert.NotNull(result);
        }
    }
}