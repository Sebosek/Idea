using System;
using System.Collections.Generic;
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
    public class RecipeServiceTests
    {
        private readonly Mock<IRepository<Recipe, Guid>> _recipeRepository;

        private readonly Mock<IRepository<Ingredient, Guid>> _ingredientRepository;

        private readonly Mock<IUnitOfWorkFactory> _factory;

        private readonly Mock<IDataProvider> _provider;

        private readonly Mock<IUnitOfWork> _uow;

        public RecipeServiceTests()
        {
            _recipeRepository = new Mock<IRepository<Recipe, Guid>>();
            _ingredientRepository = new Mock<IRepository<Ingredient, Guid>>();
            _factory = new Mock<IUnitOfWorkFactory>();
            _provider = new Mock<IDataProvider>();
            _uow = new Mock<IUnitOfWork>();

            _factory.Setup(s => s.Create()).Returns(_uow.Object);
            _factory.Setup(s => s.DataProvider()).Returns(_provider.Object);
        }

        [Fact]
        public async Task CreateRecipeAsync_ValidData_ShouldSuccess()
        {
            var ingredient = new Ingredient();
            var recipe = new Recipe { Ingredients = new List<Ingredient> { ingredient } };
            var service = new RecipeService(_factory.Object, _recipeRepository.Object, _ingredientRepository.Object);
            _ingredientRepository.Setup(s => s.FindAsync(It.IsAny<Guid>())).ReturnsAsync(ingredient);

            var result = await service.CreateRecipeAsync(recipe);

            _recipeRepository.Verify(a => a.CreateAsync(recipe));
            _ingredientRepository.Verify(a => a.UpdateAsync(ingredient));
            _uow.Verify(a => a.CommitAsync());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateRecipeAsync_ValidData_ShouldSuccess()
        {
            var before = new Recipe { Name = "before", Directions = "before", Ingredients = new List<Ingredient>() };
            var after = new Recipe { Name = "after", Directions = "after", Ingredients = new List<Ingredient>() };
            var service = new RecipeService(_factory.Object, _recipeRepository.Object, _ingredientRepository.Object);
            _recipeRepository.Setup(s => s.FindAsync(It.IsAny<Guid>())).ReturnsAsync(before);

            var result = await service.UpdateRecipeAsync(Guid.Empty, after);

            _recipeRepository.Verify(a => a.UpdateAsync(It.IsAny<Recipe>()));
            _uow.Verify(a => a.CommitAsync());
            Assert.Equal("after", result.Name);
            Assert.Equal("after", result.Directions);
        }

        [Fact]
        public async Task RemoveRecipeAsync_ValidData_ShouldSuccess()
        {
            var service = new RecipeService(_factory.Object, _recipeRepository.Object, _ingredientRepository.Object);
            _ingredientRepository.Setup(s => s.FindAsync(It.IsAny<Guid>())).ReturnsAsync(new Ingredient());

            await service.RemoveRecipeAsync(Guid.Empty);

            _recipeRepository.Verify(a => a.DeleteAsync(It.IsAny<Recipe>()));
            _uow.Verify(a => a.CommitAsync());
        }

        [Fact]
        public async Task GetRecipesAsync_ValidData_ShouldSuccess()
        {
            var recipes = new[] { new Recipe() };
            var service = new RecipeService(_factory.Object, _recipeRepository.Object, _ingredientRepository.Object);
            _provider.Setup(s => s.Data<Recipe, Guid>()).Returns(recipes.AsQueryable());

            var result = await service.GetRecipesAsync();

            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetRecipeAsync_ValidData_ShouldSuccess()
        {
            var id = Guid.NewGuid();
            var recipes = new[] { new Recipe { Id = id } };
            var service = new RecipeService(_factory.Object, _recipeRepository.Object, _ingredientRepository.Object);
            _provider.Setup(s => s.Data<Recipe, Guid>()).Returns(recipes.AsQueryable());

            var result = await service.GetRecipeAsync(id);

            Assert.NotNull(result);
        }
    }
}