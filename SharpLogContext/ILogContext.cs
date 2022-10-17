using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SharpLogContext
{
    public interface ILogContext : IDictionary<string, object>
    {
        void AttachValues(IDictionary<string, object> dictionary);
        void AttachValues(params ValueTuple<string, object>[] valueTuples);
        void AttachValues(params Tuple<string, object>[] tuples);
        void AttachValues(IEnumerable<KeyValuePair<string, object>> keyValuePairs);
        void AttachValue(KeyValuePair<string, object> keyValuePair);
        void AttachValue(string key, object value);
        void RemoveKey(string key);
        void RemoveKeys(IEnumerable<string> keys);

        ImmutableDictionary<string, object> GetValues();
        object GetValue(string key);
    }
}
