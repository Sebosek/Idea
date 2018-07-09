using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Idea.UnitOfWork.Exceptions;
using Idea.UnitOfWork.Tests.Stubs;
using Idea.UnitOfWork.Tests.Utils;

using Moq;

using Xunit;

namespace Idea.UnitOfWork.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Unit tests name convention")]
    public class UnitOfWorkGenerationTests
    {
        [Fact]
        public void Add_ValidState_ShouldSuccess()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            var generation = new UnitOfWorkGeneration();

            generation.Add(uow);

            Assert.Equal(1, generation.Index);
        }

        [Fact]
        public void CanCommit_EmptyGeneration_ShouldSuccess()
        {
            var generation = new UnitOfWorkGeneration();
            
            Assert.False(generation.CanCommit());
        }

        [Fact]
        public void CanCommit_CommitedUnitOfWork_ShouldSuccess()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            var uow2 = new UnitOfWork(manager.Object) { IsCommited = true };
            var generation = new UnitOfWorkGeneration();
            generation.Add(uow);
            generation.Add(uow2);

            Assert.True(generation.CanCommit());
        }

        [Fact]
        public async Task CommitAsync_CommitedUnitOfWork_ShouldSuccess()
        {
            (bool First, bool Second) commits = (false, false);
            var manager = new Mock<IUnitOfWorkManager>();
            var first = new UnitOfWorkStub(manager.Object, () => Tasks.FromAction(() => commits.First = true), null);
            var second = new UnitOfWorkStub(manager.Object, () => Tasks.FromAction(() => commits.Second = true), null)
            {
                IsCommited = true
            };
            var generation = new UnitOfWorkGeneration();
            generation.Add(first);
            generation.Add(second);
            manager.Setup(s => s.CanCommit()).Returns(true);
            manager.Setup(s => s.Current()).Returns(second);

            await generation.CommitAsync();

            Assert.Equal((false, true), commits);
        }

        [Fact]
        public void CommitAsync_EmptyGeneration_ShouldThrowException()
        {
            var generation = new UnitOfWorkGeneration();

            Assert.Throws<NoUnitOfWorkInGenerationException>(() => generation.CloseCurrent());
        }

        [Fact]
        public void CommitAsync_CommitedGeneration_ShouldSuccess()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object) { IsCommited = true };
            var generation = new UnitOfWorkGeneration();
            generation.Add(uow);

            generation.CloseCurrent();

            Assert.False(uow.IsOpen);
        }

        [Fact]
        public void Current_EmptyGeneration_ShouldThrowException()
        {
            var generation = new UnitOfWorkGeneration();

            Assert.Throws<NoUnitOfWorkInGenerationException>(() => generation.CloseCurrent());
        }

        [Fact]
        public void Current_FilledUnitOfWork_ShouldThrowException()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            var generation = new UnitOfWorkGeneration();
            generation.Add(uow);

            Assert.NotNull(generation.Current());
        }

        [Fact]
        public void CleanUp_CommitedUnitOfWork_ShouldSuccess()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            var uow2 = new UnitOfWork(manager.Object) { IsCommited = true };
            var generation = new UnitOfWorkGeneration();
            generation.Add(uow);
            generation.Add(uow2);

            generation.CleanUp();

            Assert.True(generation.Stack.All(a => a == null));
        }
    }
}