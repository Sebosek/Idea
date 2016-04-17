using System;

using Moq;
using Xunit;

namespace Idea7.UnitOfWork.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void CreateInstance_ShouldSuccess()
        {
            var manager = CreateManager();
            var uow = new UnitOfWork(manager);

            Assert.NotNull(uow);
        }

        [Fact]
        public void CreateInstanceInsideUsing_ShouldSuccess()
        {
            var manager = CreateManager();
            using (var uow = new UnitOfWork(manager))
            {
                Assert.NotNull(uow);
                Assert.True(uow.IsOpen);
            }
        }

        [Fact]
        public void CreateInstanceCheckAddToManager_ShouldSuccess()
        {
            var manager = CreateManager();
            var uow = new UnitOfWork(manager);

            Mock.Get(manager).Verify(v => v.Add(It.IsAny<IUnitOfWork>()));
        }

        [Fact]
        public void Commit_ShouldSuccess()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            uow.Commit();

            Assert.False(uow.IsOpen);
        }

        [Fact]
        public void CommitOutsideUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            moq.Setup(m => m.Add(It.IsAny<IUnitOfWork>())).Verifiable();

            using (var uow = new UnitOfWork(moq.Object))
            {
                using (var uow2 = new UnitOfWork(moq.Object))
                {
                    moq.Setup(m => m.Current()).Returns(uow2);

                    var ex = Assert.Throws<Exception>(() => uow.Commit());
                    Assert.Equal(ex.Message, "Try to commit outside Unit of work.");
                }
            }
        }

        [Fact]
        public void CommitNotOpenedUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            // do something
            uow.Commit();

            var ex = Assert.Throws<Exception>(() => uow.Commit());
            Assert.Equal(ex.Message, "Unit of work isn't open.");
        }

        [Fact]
        public void Rollback_ShouldSuccess()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            uow.Rollback();

            Assert.False(uow.IsOpen);
        }

        [Fact]
        public void RollbackOutsideUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            moq.Setup(m => m.Add(It.IsAny<IUnitOfWork>())).Verifiable();

            using (var uow = new UnitOfWork(moq.Object))
            {
                using (var uow2 = new UnitOfWork(moq.Object))
                {
                    moq.Setup(m => m.Current()).Returns(uow2);

                    var ex = Assert.Throws<Exception>(() => uow.Rollback());
                    Assert.Equal(ex.Message, "Try to rollback outside Unit of work.");
                }
            }
        }

        [Fact]
        public void RollbackNotOpenedUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWork(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            // do something
            uow.Rollback();

            var ex = Assert.Throws<Exception>(() => uow.Rollback());
            Assert.Equal(ex.Message, "Unit of work isn't open.");
        }

        [Fact]
        public void EqualsEqualedUow_ShouldSuccess()
        {
            var manager = CreateManager();
            var uow = new UnitOfWork(manager);
            var moq = new Mock<IUnitOfWork>();
            moq.SetupGet(g => g.Id).Returns(uow.Id);

            Assert.True(uow.Equals(moq.Object));
        }

        [Fact]
        public void EqualsNonequleUow_ShouldReturnFalse()
        {
            var manager = CreateManager();
            var uow = new UnitOfWork(manager);
            var uow2 = new UnitOfWork(manager);

            Assert.False(uow.Equals(uow2));
        }

        private IUnitOfWorkManager CreateManager(IUnitOfWork used = null)
        {
            var moq = new Mock<IUnitOfWorkManager>();
            moq.Setup(m => m.Add(It.IsAny<IUnitOfWork>())).Verifiable();
            moq.Setup(m => m.Current()).Returns(used).Verifiable();

            return moq.Object;
        }
    }
}
