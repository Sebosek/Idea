using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork.Tests.Stubs
{
    public class UnitOfWorkStub : UnitOfWork
    {
        private readonly Func<Task> _commit;

        private readonly Func<Task> _rollback;

        public UnitOfWorkStub(IUnitOfWorkManager manager, Func<Task> commit, Func<Task> rollback)
            : base(manager)
        {
            _commit = commit;
            _rollback = rollback;
        }

        protected override Task DoCommitAsync() => _commit();

        protected override Task DoRollbackAsync() => _rollback();
    }
}