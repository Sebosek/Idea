using System;
using Xunit;

namespace Idea7.UnitOfWork.Tests
{
    public class UnitOfWorkManagerTests
    {
        private const string UoWId = "871a0af9-f3ef-47b0-a303-ca4a2bde52ba";

        [Fact]
        public void CreateInstance_ShouldSucess()
        {
            var manager = new UnitOfWorkManager();

            Assert.NotNull(manager);
            Assert.IsAssignableFrom<IUnitOfWorkManager>(manager);
        }

        [Fact]
        public void Add_ShouldSuccess()
        {
            var manager = new UnitOfWorkManager();
            var uow = CreateUnitOfWork();

            manager.Add(uow);
            var current = manager.Current();

            Assert.Equal(uow.Id, current.Id);
        }

        [Fact]
        public void Current_ShouldSuccess()
        {
            var manager = new UnitOfWorkManager();
            var uow = CreateUnitOfWork();

            manager.Add(uow);
            var current = manager.Current();

            Assert.Equal(uow.Id, current.Id);
        }

        [Fact]
        public void CurrentEmptyStact_ShouldThrowException()
        {
            var manager = new UnitOfWorkManager();

            Exception ex = Assert.Throws<Exception>(() => manager.Current());

            Assert.Equal(ex.Message, "None Unit of Work is currently open.");
        }

        [Fact]
        public void CloseNonEmptyStact_ShouldSuccess()
        {
            var manager = new UnitOfWorkManager();
            var uow = CreateUnitOfWork();

            manager.Add(uow);

            var before = manager.Stack[0];
            manager.Close();
            var after = manager.Stack[0];

            Assert.NotNull(before);
            Assert.Null(after);
        }

        [Fact]
        public void CloseEmptyStact_ShouldThrowException()
        {
            var manager = new UnitOfWorkManager();

            Exception ex = Assert.Throws<Exception>(() => manager.Close());

            Assert.Equal(ex.Message, "None Unit of Work is currently open.");
        }

        private IUnitOfWork CreateUnitOfWork()
        {
            var uow = new Moq.Mock<IUnitOfWork>();
            uow.Setup(p => p.Id).Returns(UoWId);
            uow.Setup(p => p.IsCommited).Returns(false);

            return uow.Object;
        }
    }
}
