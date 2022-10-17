using System;
using System.Collections.Generic;

namespace SharpLogContext
{
    public interface IScopedLogContext : ILogContext
    {
        IDisposable CreateScope(string key, object value);

        IDisposable CreateScope(IEnumerable<KeyValuePair<string, object>> keyValuePairs);

        IDisposable CreateScope(KeyValuePair<string, object> keyValuePair);

        IDisposable CreateScope(params ValueTuple<string, object>[] valueTuples);

        IDisposable CreateScope(params Tuple<string, object>[] tuples);

        IDisposable CreateScope(Action<ILogContext> buildContextAction);
    }
}
