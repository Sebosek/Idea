using System.Threading.Tasks;

using Idea.UnitOfWork.Exceptions;

namespace Idea.UnitOfWork
{
    public class UnitOfWorkGeneration : IUnitOfWorkGeneration
    {
        private static int RANGE = 32;

        private int _height;

        public UnitOfWorkGeneration()
        {
            Index = 0;
            Stack = new UnitOfWork[RANGE];
        }

        public bool AllClosed => Index == 0;

        protected internal int Index { get; private set; }

        protected internal UnitOfWork[] Stack { get; }

        public void Add(UnitOfWork uow)
        {
            Stack[_height++] = uow;
            Index++;
        }

        public bool CanCommit()
        {
            // Need to check if any of unit of works was commited.
            // With that check, we know that one or more generation wasn't skipped on the vertical axis.

            var i = 0;
            while (i < RANGE && Stack[i] != null)
            {
                if (Stack[i].IsCommited)
                {
                    return true;
                }

                i++;
            }

            return false;
        }

        public async Task CommitAsync()
        {
            var i = 0;
            while (i < RANGE && Stack[i] != null)
            {
                if (Stack[i].IsCommited)
                {
                    await Stack[i].InternalCommitAsync();
                }

                i++;
            }
        }

        public void CloseCurrent()
        {
            CheckEmpty();

            Stack[--Index].IsOpen = false;
        }

        public UnitOfWork Current()
        {
            return Stack[_height - 1];

            ////CheckEmpty();
            ////return Stack[--Index];
        }

        public void CleanUp()
        {
            var j = 0;
            while (j < RANGE && Stack[j] != null)
            {
                Release(Stack[j]);
                Stack[j] = null;
                j++;
            }
        }

        protected virtual void Release(IUnitOfWork uow) { }

        private void CheckEmpty()
        {
            if (Index == 0)
            {
                throw new NoUnitOfWorkInGenerationException("No unit of work is in a generation.");
            }
        }
    }
}
