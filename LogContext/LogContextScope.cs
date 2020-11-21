using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LogContext
{
    internal class LogContextScope : IDisposable
    {
        private readonly ImmutableArray<string> _keys;
        private readonly LogContext _parent;
        private readonly IImmutableDictionary<string, object> _prevState;

        public LogContextScope(LogContext parent, IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            _parent = parent ?? throw new ArgumentException(nameof(parent));
            if (keyValuePairs == null)
                throw new ArgumentException(nameof(keyValuePairs));
            var valuePairs = keyValuePairs as KeyValuePair<string, object>[] ?? keyValuePairs.ToArray();
            _keys = valuePairs.Select(p => p.Key).ToImmutableArray();
            var prevValues = parent.GetValues();
            _prevState = _keys.Where(k => prevValues.ContainsKey(k))
               .Select(k => new KeyValuePair<string, object>(k, prevValues[k]))
               .ToImmutableDictionary(p => p.Key, p => p.Value);
            _parent.AttachValues(valuePairs);
        }

        public void Dispose()
        {
            _parent.RemoveKeys(_keys);
            _parent.AttachValues(_prevState);
        }
    }
}