using System;

namespace Idea.UnitOfWork.Exceptions
{
    public class IdeaException : Exception
    {
        public IdeaException()
        {
        }

        public IdeaException(string message)
            : base(message)
        {
        }

        public IdeaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}