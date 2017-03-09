using System;

using Idea.SmartQuery.Interfaces;

namespace Idea.SmartQuery
{
    public class QueryReader<TQueryData> : IQueryReader<TQueryData>
        where TQueryData : IQueryData
    {
        private readonly Func<TQueryData> _read;

        public QueryReader(Func<TQueryData> read)
        {
            _read = read;
        }

        public TQueryData Read()
        {
            return _read.Invoke();
        }
    }
}