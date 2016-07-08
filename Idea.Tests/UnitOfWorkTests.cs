using System;
using Idea.UnitOfWork;
using Moq;
using Xunit;
using UnitOfWorkObject = Idea.UnitOfWork.UnitOfWork;

namespace Idea.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void CreateInstance_ShouldSuccess()
        {
            var manager = CreateManager();
            var uow = new UnitOfWorkObject(manager);

            Assert.NotNull(uow);
            Assert.True(uow.IsOpen);
        }

        [Fact]
        public void CreateInstanceCheckAddToManager_ShouldSuccess()
        {
            var manager = CreateManager();
            var uow = new UnitOfWorkObject(manager);

            Mock.Get(manager).Verify(v => v.Add(It.IsAny<IUnitOfWork>()));
        }

        [Fact]
        public void Commit_ShouldSuccess()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWorkObject(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            uow.Commit();

            Assert.False(uow.IsOpen);
        }

        [Fact]
        public void CommitOutsideUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            moq.Setup(m => m.Add(It.IsAny<IUnitOfWork>())).Verifiable();

            using (var uow = new UnitOfWorkObject(moq.Object))
            {
                using (var uow2 = new UnitOfWorkObject(moq.Object))
                {
                    moq.Setup(m => m.Current()).Returns(uow2);

                    Assert.Throws<Exception>(() => uow.Commit());

                    moq.Setup(m => m.Current()).Returns(uow2);
                }

                moq.Setup(m => m.Current()).Returns(uow);
            }
        }

        [Fact]
        public void CommitNotOpenedUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWorkObject(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            // do something
            uow.Commit();

            Assert.Throws<Exception>(() => uow.Commit());
        }

        [Fact]
        public void Rollback_ShouldSuccess()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWorkObject(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            uow.Rollback();

            Assert.False(uow.IsOpen);
        }

        [Fact]
        public void RollbackOutsideUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            moq.Setup(m => m.Add(It.IsAny<IUnitOfWork>())).Verifiable();

            using (var uow = new UnitOfWorkObject(moq.Object))
            {
                using (var uow2 = new UnitOfWorkObject(moq.Object))
                {
                    moq.Setup(m => m.Current()).Returns(uow2);

                    Assert.Throws<Exception>(() => uow.Rollback());

                    moq.Setup(m => m.Current()).Returns(uow2);
                }

                moq.Setup(m => m.Current()).Returns(uow);
            }
        }

        [Fact]
        public void RollbackNotOpenedUoW_ShouldThrowException()
        {
            var moq = new Mock<IUnitOfWorkManager>();
            var uow = new UnitOfWorkObject(moq.Object);
            moq.Setup(m => m.Current()).Returns(uow);

            // do something
            uow.Rollback();

            Assert.Throws<Exception>(() => uow.Rollback());
        }

        [Fact]
        public void EqualsEqualedUow_ShouldSuccess()
        {
            var manager = CreateManager();
            var uow = new UnitOfWorkObject(manager);
            var moq = new Mock<IUnitOfWork>();
            moq.SetupGet(g => g.Id).Returns(uow.Id);

            Assert.True(uow.Equals(moq.Object));
        }

        [Fact]
        public void EqualsNonequleUow_ShouldReturnFalse()
        {
            var manager = CreateManager();
            var uow = new UnitOfWorkObject(manager);
            var uow2 = new UnitOfWorkObject(manager);

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
