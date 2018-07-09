using System.Diagnostics.CodeAnalysis;

using Idea.UnitOfWork.Exceptions;

using Moq;

using Xunit;

namespace Idea.UnitOfWork.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Unit tests name convention")]
    public class UnitOfWorkManagerTests
    {
        private readonly Mock<IUnitOfWorkGenerationFactory> _factory;

        private readonly Mock<IUnitOfWorkGeneration> _generation;

        private readonly UnitOfWorkManager _manager;

        public UnitOfWorkManagerTests()
        {
            _factory = new Mock<IUnitOfWorkGenerationFactory>();
            _generation = new Mock<IUnitOfWorkGeneration>();
            _manager = new UnitOfWorkManager(_factory.Object);

            _factory.Setup(s => s.Create()).Returns(_generation.Object);
        }

        [Fact]
        public void Add_ValidState_ShouldSucceed()
        {
            var uow = new UnitOfWork(_manager);

            _manager.Add(uow);

            Assert.NotEmpty(_manager.Stack);
        }

        [Fact]
        public void Add_MaximumDepth_ShouldThrowException()
        {
            var uow = new UnitOfWork(_manager);

            Assert.Throws<ReachedMaximumDepthException>(() =>
            {
                for (var i = 0; i < UnitOfWorkManager.Depth; i++)
                {
                    _manager.Add(uow);
                }
            });
        }

        [Fact]
        public void CanCommit_EmptyStack_ShouldReturnTrue()
        {
            Assert.True(_manager.CanCommit());
        }

        [Fact]
        public void CanCommit_OneUnitOfWork_ShouldReturnTrue()
        {
            new UnitOfWork(_manager);
            _generation.Setup(s => s.CanCommit()).Returns(true);

            Assert.True(_manager.CanCommit());
        }

        [Fact]
        public void CanCommit_MoreUnitOfWorks_ShouldReturnFalse()
        {
            new UnitOfWork(_manager);
            new UnitOfWork(_manager);
            _generation.Setup(s => s.CanCommit()).Returns(true);

            Assert.False(_manager.CanCommit());
        }
    }
}