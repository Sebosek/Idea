namespace Idea.SmartQuery.Interfaces
{
    public interface IQueryReader<out TQueryData>
        where TQueryData : IQueryData
    {
        TQueryData Read();
    }
}