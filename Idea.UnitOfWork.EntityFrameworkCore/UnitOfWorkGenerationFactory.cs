namespace Idea.UnitOfWork.EntityFrameworkCore
{
    public class UnitOfWorkGenerationFactory : IUnitOfWorkGenerationFactory
    {
        public IUnitOfWorkGeneration Create()
        {
            return new UnitOfWorkGeneration();
        }
    }
}
