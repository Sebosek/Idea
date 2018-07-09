using System;
using System.Threading.Tasks;

namespace Idea.UnitOfWork.Tests.Utils
{
    internal static class Tasks
    {
        public static Task FromAction(Action action)
        {
            action();

            return Task.CompletedTask;
        }
    }
}