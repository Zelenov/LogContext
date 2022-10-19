using System;
using System.Collections.Generic;

namespace SharpLogContext.Abstractions;

/// <summary>
/// <see cref="IDictionary{TKey,TValue}"/> with additional functionality
/// </summary>
public interface ILogContext : IDictionary<string, object>
{
    void Add(IDictionary<string, object> dictionary);
    void Add(params ValueTuple<string, object>[] valueTuples);
    void Add(params Tuple<string, object>[] tuples);
    void Add(IEnumerable<KeyValuePair<string, object>> keyValuePairs);
    void RemoveKeys(IEnumerable<string> keys);
}