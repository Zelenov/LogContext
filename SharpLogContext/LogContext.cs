using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace SharpLogContext
{
    /// <summary>
    /// Stores dictionary-like data between all the components in the scope of one .NET Core request
    /// Remember to call <see cref="ApplicationBuilderExtensions.UseLogContext"/> or <see cref="CreateNewLogContext"/>
    /// to get rid of conflicts between different requests accessing the same context
    /// </summary>
    public class LogContext
    {
        private static readonly object Lock = new object();
        private static readonly AsyncLocal<LogContext> CurrentLogContext = new AsyncLocal<LogContext>();

        private readonly Lazy<ConcurrentDictionary<string, object>> _attachedValues =
            new Lazy<ConcurrentDictionary<string, object>>(() => new ConcurrentDictionary<string, object>());

        private LogContext()
        {
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
                    _attachedValues.Value.AddOrUpdate(key, value, (k, oldValue) => value);
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
                    _attachedValues.Value.TryRemove(key, out _);
        }

        /// <summary>
        /// Retrieve all values from the dictionary
        /// </summary>
        public ImmutableDictionary<string, object> GetValues() => _attachedValues.Value.ToImmutableDictionary();

        /// <summary>
        /// Retrieve value from the dictionary
        /// </summary>
        /// <returns>value, <see cref="key"/> exists, default otherwise</returns>
        public object GetValue(string key)
        {
            TryGetValue(key, out var res);
            return res;
        }

        /// <summary>
        /// Try retrieve all values from the dictionary
        /// </summary>
        /// <returns>true if value with <see cref="key"/> exists, default otherwise</returns>
        public bool TryGetValue(string key, out object value) => _attachedValues.Value.TryGetValue(key, out value);

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
    }
}