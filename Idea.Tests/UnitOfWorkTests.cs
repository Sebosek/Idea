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
