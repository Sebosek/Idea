using System;

namespace Idea.UnitOfWork.Exceptions
{
    public class ReachedMaximumDepthException : IdeaException
    {
        public ReachedMaximumDepthException()
        {
        }

        public ReachedMaximumDepthException(string message)
            : base(message)
        {
        }

        public ReachedMaximumDepthException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}