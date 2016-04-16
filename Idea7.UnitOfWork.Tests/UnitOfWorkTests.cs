using System.Collections.Generic;
using System.Linq;

using Moq;
using Xunit;

namespace Idea7.UnitOfWork.Tests
{
    public class UnitOfWorkTests
    {
        private IList<IUnitOfWork> _units = new List<IUnitOfWork>();

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
            Assert.True(_units.Any(a => a.Id == uow.Id));
        }

        [Fact]
        public void Commit_ShouldSuccess()
        {
            var manager = CreateManager();
            var uow = new UnitOfWork(manager);
        }

        private IUnitOfWorkManager CreateManager(IUnitOfWork used = null)
        {
            var moq = new Mock<IUnitOfWorkManager>();
            moq.Setup(m => m.Add(It.IsAny<IUnitOfWork>()))
               .Callback<IUnitOfWork>((uow) =>
                {
                    _units.Add(uow);
                })
               .Verifiable();
            moq.Setup(m => m.Current()).Returns(used).Verifiable();

            return moq.Object;
        }
    }
}
