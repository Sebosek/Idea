using System;

namespace Idea.UnitOfWork.Exceptions
{
    public class NoUnitOfWorkInGenerationException : IdeaException
    {
        public NoUnitOfWorkInGenerationException()
        {
        }

        public NoUnitOfWorkInGenerationException(string message)
            : base(message)
        {
        }

        public NoUnitOfWorkInGenerationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}