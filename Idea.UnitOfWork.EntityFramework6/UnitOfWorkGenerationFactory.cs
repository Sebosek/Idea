namespace Idea.UnitOfWork.EntityFramework6
{
    public class UnitOfWorkGenerationFactory : IUnitOfWorkGenerationFactory
    {
        public IUnitOfWorkGeneration Create()
        {
            return new UnitOfWorkGeneration();
        }
    }
}
