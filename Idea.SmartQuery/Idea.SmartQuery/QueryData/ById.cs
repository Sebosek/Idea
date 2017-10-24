using Idea.SmartQuery.Interfaces;

namespace Idea.SmartQuery.QueryData
{
    public class ById<T> : IQueryData
    {
        public T Id { get; }

        public ById(T id)
        {
            Id = id;
        }
    }
}