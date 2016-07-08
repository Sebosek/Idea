using System;
using Idea.UnitOfWork;
using Xunit;

namespace Idea.Tests
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
        public void Add_NewUnitOfWork_ShouldSuccess()
        {
            var manager = new UnitOfWorkManager();
            var uow = CreateUnitOfWork();

            manager.Add(uow);

            var current = manager.Current();
            Assert.Equal(uow.Id, current.Id);
        }

        [Fact]
        public void Current_EmptyStact_ShouldThrowException()
        {
            var manager = new UnitOfWorkManager();

            Exception ex = Assert.Throws<Exception>(() => manager.Current());

            Assert.Equal(ex.Message, "None Unit of Work is currently open.");
        }

        [Fact]
        public void Close_NonEmptyStact_ShouldSuccess()
        {
            var manager = new UnitOfWorkManager();
            var uow = new UnitOfWork.UnitOfWork(manager);
            manager.Add(uow);

            var before = ((UnitOfWork.UnitOfWork) manager.Stack[0]).IsOpen;
            manager.Close();
            var after = ((UnitOfWork.UnitOfWork)manager.Stack[0]).IsOpen;

            Assert.True(before);
            Assert.False(after);
        }

        [Fact]
        public void Close_EmptyStact_ShouldThrowException()
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
