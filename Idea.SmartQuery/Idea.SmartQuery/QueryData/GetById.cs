using Idea.SmartQuery.Interfaces;

namespace Idea.SmartQuery.QueryData
{
    internal class GetById<T> : IQueryData
    {
        public T Id { get; }

        public GetById(T id)
        {
            Id = id;
        }
    }
}