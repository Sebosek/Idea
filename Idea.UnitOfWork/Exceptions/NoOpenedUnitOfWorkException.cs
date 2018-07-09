using System;

namespace Idea.UnitOfWork.Exceptions
{
    public class NoOpenedUnitOfWorkException : IdeaException
    {
        public NoOpenedUnitOfWorkException()
        {
        }

        public NoOpenedUnitOfWorkException(string message)
            : base(message)
        {
        }

        public NoOpenedUnitOfWorkException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}