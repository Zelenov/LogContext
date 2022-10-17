using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace SharpLogContext
{
    /// <summary>
    /// Stores dictionary-like data between all the components in the scope of one .NET Core request
    /// Remember to call <see cref="CreateNewLogContext"/>
    /// to get rid of conflicts between different requests accessing the same context
    /// </summary>
    public class LogContext: IScopedLogContext
    {
        private static readonly object Lock = new object();
        private static readonly AsyncLocal<LogContext> CurrentLogContext = new AsyncLocal<LogContext>();

        private readonly Lazy<IDictionary<string, object>> _attachedValues =
            new Lazy<IDictionary<string, object>>(() => new ConcurrentDictionary<string, object>());

        private IDictionary<string, object> LogContextImplementation => _attachedValues.Value;

        private LogContext()
        {
        }

        /// <summary>
        /// Clears context
        /// </summary>
        public static void Clear()
        {
            lock (Lock)
            {
                CurrentLogContext.Value = null;
            }
        }

        /// <summary>
        /// Current log context for the request
        /// </summary>
        public static LogContext Current
        {
            get
            {
                lock (Lock)
                {
                    var logContext = CurrentLogContext.Value;
                    if (logContext == null)
                        CurrentLogContext.Value = logContext = new LogContext();
                    return logContext;
                }
            }
        }

        /// <summary>
        /// Renew <see cref="Current"/> context
        /// </summary>
        /// <returns></returns>
        public static LogContext CreateNewLogContext()
        {
            lock (Lock)
            {
                var logContext = new LogContext();
                CurrentLogContext.Value = logContext;
                return logContext;
            }
        }
        /// <summary>
        /// Attach values available only while result is not disposed
        /// </summary>
        /// <remarks>
        /// All rewritten values would be restored on scope dispose
        /// </remarks>
        /// <returns>scope</returns>
        public IDisposable CreateScope(string key, object value)
        {
            return CreateScope(new[] {new KeyValuePair<string, object>(key, value)});
        }

        /// <summary>
        /// Attach values available only while result is not disposed
        /// </summary>
        /// <remarks>
        /// All rewritten values would be restored on scope dispose
        /// </remarks>
        /// <returns>scope</returns>
        public IDisposable CreateScope(IEnumerable<KeyValuePair<string, object>> keyValuePairs) =>
            new LogContextScope(this, keyValuePairs);

        /// <summary>
        /// Attach values available only while result is not disposed
        /// </summary>
        /// <remarks>
        /// All rewritten values would be restored on scope dispose
        /// </remarks>
        /// <returns>scope</returns>
        public IDisposable CreateScope(KeyValuePair<string, object> keyValuePair) =>
            CreateScope(new []{keyValuePair});

        /// <summary>
        /// Attach values available only while result is not disposed
        /// </summary>
        /// <remarks>
        /// All rewritten values would be restored on scope dispose
        /// </remarks>
        /// <returns>scope</returns>
        public IDisposable CreateScope(params ValueTuple<string, object>[] valueTuples) =>
            CreateScope(valueTuples.Select(t => new KeyValuePair<string, object>(t.Item1, t.Item2)));
        /// <summary>
        /// Attach values available only while result is not disposed
        /// </summary>
        /// <remarks>
        /// All rewritten values would be restored on scope dispose
        /// </remarks>
        /// <returns>scope</returns>
        public IDisposable CreateScope(params Tuple<string, object>[] tuples) =>
            CreateScope(tuples.Select(t => new KeyValuePair<string, object>(t.Item1, t.Item2)));


        public IDisposable CreateScope(Action<ILogContext> buildContextAction)
        {
            var tempLogContext = new LogContext();
            buildContextAction?.Invoke(tempLogContext);
            return CreateScope(tempLogContext);
        }

        /// <summary>
        /// Attach values to the context dictionary
        /// </summary>
        public void AttachValues(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentException(nameof(dictionary));
            AttachValues((IEnumerable<KeyValuePair<string, object>>) dictionary);
        }

        /// <summary>
        /// Attach values to the context dictionary
        /// </summary>
        public void AttachValues(params ValueTuple<string, object>[] valueTuples)
        {
            if (valueTuples == null)
                throw new ArgumentException(nameof(valueTuples));
            AttachValues(valueTuples.Select(t => new KeyValuePair<string, object>(t.Item1, t.Item2)));
        }
        /// <summary>
        /// Attach values to the context dictionary
        /// </summary>
        public void AttachValues(params Tuple<string, object>[] tuples)
        {
            if (tuples == null)
                throw new ArgumentException(nameof(tuples));
            AttachValues(tuples.Select(t => new KeyValuePair<string, object>(t.Item1, t.Item2)));
        }

        /// <summary>
        /// Attach values to the context dictionary
        /// </summary>
        public void AttachValues(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            if (keyValuePairs == null)
                throw new ArgumentException(nameof(keyValuePairs));
            foreach (var keyValuePair in keyValuePairs)
            {
                var key = keyValuePair.Key;
                var value = keyValuePair.Value;
                if (!string.IsNullOrWhiteSpace(key))
                    LogContextImplementation[key] = value;
            }
        }

        /// <summary>
        /// Attach value to the context dictionary
        /// </summary>
        public void AttachValue(KeyValuePair<string, object> keyValuePair)
        {
            AttachValues(new[] {keyValuePair});
        }

        /// <summary>
        /// Attach value to the context dictionary
        /// </summary>
        public void AttachValue(string key, object value)
        {
            AttachValues(new[] {new KeyValuePair<string, object>(key, value)});
        }

        /// <summary>
        /// Remove key from dictionary
        /// </summary>
        /// <remarks>
        /// If method was called inside the scope that rewrote the removed key, it would be restored on scope dispose.
        /// </remarks>
        public void RemoveKey(string key)
        {
            if (key == null)
                throw new ArgumentException(nameof(key));
            RemoveKeys(new[] {key});
        }

        /// <summary>
        /// Remove keys from dictionary
        /// </summary>
        /// <remarks>
        /// If method was called inside the scope that rewrote the removed key, it would be restored on scope dispose.
        /// </remarks>
        public void RemoveKeys(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentException(nameof(keys));
            foreach (var key in keys)
                if (!string.IsNullOrWhiteSpace(key))
                    LogContextImplementation.Remove(key);
        }

        /// <summary>
        /// Retrieve all values from the dictionary
        /// </summary>
        public ImmutableDictionary<string, object> GetValues() => LogContextImplementation.ToImmutableDictionary();

        /// <summary>
        /// Retrieve value from the dictionary
        /// </summary>
        /// <returns>value, <see cref="key"/> exists, default otherwise</returns>
        public object GetValue(string key)
        {
            TryGetValue(key, out var res);
            return res;
        }

        public void Add(string key, object value)
        {
            LogContextImplementation.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return LogContextImplementation.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return LogContextImplementation.Remove(key);
        }

        /// <summary>
        /// Try retrieve all values from the dictionary
        /// </summary>
        /// <returns>true if value with <see cref="key"/> exists, default otherwise</returns>
        public bool TryGetValue(string key, out object value) => LogContextImplementation.TryGetValue(key, out value);

        public object this[string key]
        {
            get => LogContextImplementation[key];
            set => LogContextImplementation[key] = value;
        }

        public ICollection<string> Keys => LogContextImplementation.Keys;

        public ICollection<object> Values => LogContextImplementation.Values;

        /// <summary>
        /// Retrieve value from the dictionary and cast it to <see cref="T"/>
        /// </summary>
        /// <returns><see cref="T"/> if values value of type <see cref="T"/> exists, default otherwise</returns>
        public T GetValueAs<T>(string key)
        {
            if (!TryGetValue(key, out var value))
                return default;

            return value is T value1 ? value1 : default;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return LogContextImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)LogContextImplementation).GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            LogContextImplementation.Add(item);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            LogContextImplementation.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return LogContextImplementation.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            LogContextImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return LogContextImplementation.Remove(item);
        }

        public int Count => LogContextImplementation.Count;

        public bool IsReadOnly => LogContextImplementation.IsReadOnly;
    }
}