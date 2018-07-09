using System;

namespace Idea.UnitOfWork.Exceptions
{
    public class CommitOuterUnitOfWorkException : IdeaException
    {
        public CommitOuterUnitOfWorkException()
        {
        }

        public CommitOuterUnitOfWorkException(string message)
            : base(message)
        {
        }

        public CommitOuterUnitOfWorkException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}