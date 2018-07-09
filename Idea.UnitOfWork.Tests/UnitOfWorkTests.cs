using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Idea.UnitOfWork.Exceptions;

using Moq;

using Xunit;

namespace Idea.UnitOfWork.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Unit tests name contevtion")]
    public class UnitOfWorkTests
    {
        [Fact]
        public async Task CommitAsync_ValidState_ShouldSuccess()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            manager.Setup(s => s.Current()).Returns(uow);
            manager.Setup(s => s.CanCommit()).Returns(true);

            await uow.CommitAsync();

            Assert.True(uow.IsCommited);
            manager.Verify(a => a.CommitAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CommitAsync_UnableCommit_ShouldSkipCommitAll()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            manager.Setup(s => s.Current()).Returns(uow);
            manager.Setup(s => s.CanCommit()).Returns(false);

            await uow.CommitAsync();

            manager.Verify(a => a.CommitAllAsync(), Times.Never);
        }

        [Fact]
        public Task CommitAsync_NoOpened_ShouldThrowException()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object) { IsOpen = false };
            
            return Assert.ThrowsAsync<NoOpenedUnitOfWorkException>(() => uow.CommitAsync());
        }

        [Fact]
        public Task CommitAsync_OuterUnitOfWork_ShouldThrowException()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            var outer = new UnitOfWork(manager.Object);
            manager.Setup(s => s.Current()).Returns(outer);

            return Assert.ThrowsAsync<CommitOuterUnitOfWorkException>(() => uow.CommitAsync());
        }

        [Fact]
        public async Task RollbackAsync_ValidState_ShouldSuccess()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            manager.Setup(s => s.Current()).Returns(uow);
            manager.Setup(s => s.CanCommit()).Returns(true);

            await uow.RollbackAsync();

            Assert.False(uow.IsOpen);
        }

        [Fact]
        public void Dispose_ValidState_ShouldSuccess()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            manager.Setup(s => s.Current()).Returns(uow);
            manager.Setup(s => s.CanCommit()).Returns(true);

            uow.Dispose();

            Assert.False(uow.IsOpen);
            manager.Verify(a => a.Close(), Times.Once);
            manager.Verify(a => a.CleanUp(), Times.Once);
        }

        [Fact]
        public void Dispose_DoubleDispose_ShouldRunOnec()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            manager.Setup(s => s.Current()).Returns(uow);
            manager.Setup(s => s.CanCommit()).Returns(true);

            uow.Dispose();
            uow.Dispose();

            Assert.False(uow.IsOpen);
            manager.Verify(a => a.Close(), Times.Once);
            manager.Verify(a => a.CleanUp(), Times.Once);
        }

        [Fact]
        public void Equals_ValidState_ShouldBeEqual()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);

            Assert.True(uow.Equals(uow));
        }

        [Fact]
        public void Equals_DifferentInstances_ShouldNotBeEqual()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);
            var uow2 = new UnitOfWork(manager.Object);

            Assert.False(uow.Equals(uow2));
        }

        [Fact]
        public void Equals_DifferentTypeInstance_ShouldNotBeEqual()
        {
            var manager = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(manager.Object);

            Assert.False(uow.Equals(new object()));
        }
    }
}