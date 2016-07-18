namespace Idea.UnitOfWork
{
    public class UnitOfWorkGenerationFactory : IUnitOfWorkGenerationFactory
    {
        public IUnitOfWorkGeneration Create()
        {
            return new UnitOfWorkGeneration();
        }
    }
}
