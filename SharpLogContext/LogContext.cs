using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using SharpLogContext.Abstractions;

namespace SharpLogContext;

/// <summary>
/// Stores dictionary-like data between all the components in async-local scope
/// Remember to call <see cref="Initialize"/>
/// to initialize async-local value
/// </summary>
public class LogContext: ILogContext, IScopeConstructor, IGlobalLogContext
{
    private static readonly object Lock = new();
    private static readonly AsyncLocal<LogContext> CurrentLogContext = new ();
    private readonly ILogContextScopeChain _scopeChain = new LoggerContextScopeChain();
    private readonly ILogContext _rootLogContext;

    public LogContext()
    {
        _rootLogContext = new ScopedLogContext(_scopeChain);
    }

    /// <summary>
    /// Clears async-local context variable
    /// </summary>
    public static void Release()
    {
        lock (Lock)
        {
            CurrentLogContext.Value = null;
        }
    }

    /// <summary>
    /// Current log context
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
    /// Initialize <see cref="Current"/> context async-locally
    /// </summary>
    /// <returns></returns>
    public static LogContext Initialize()
    {
        lock (Lock)
        {
            var logContext = new LogContext();
            CurrentLogContext.Value = logContext;
            return logContext;
        }
    }

    public IScopedLogContext CreateScope() => 
        CreateScope(buildContextAction: null);

    public IScopedLogContext CreateScope(string key, object value) => 
        CreateScope(new[] { new KeyValuePair<string, object>(key, value) });

    public IScopedLogContext CreateScope(IEnumerable<KeyValuePair<string, object>> keyValuePairs) =>
        CreateScope(keyValuePairs.ToDictionary(x=>x.Key, x=>x.Value));

    public IScopedLogContext CreateScope(KeyValuePair<string, object> keyValuePair) =>
        CreateScope(new Dictionary<string, object>{{keyValuePair.Key, keyValuePair.Value}});

    public IScopedLogContext CreateScope(params ValueTuple<string, object>[] valueTuples) =>
        CreateScope(valueTuples.ToDictionary(x => x.Item1, x => x.Item2));

    public IScopedLogContext CreateScope(params Tuple<string, object>[] tuples) =>
        CreateScope(tuples.ToDictionary(x => x.Item1, x => x.Item2));

    public IScopedLogContext CreateScope(Action<ILogContext> buildContextAction)
    {
        var innerLogContext = new ScopedLogContext(_scopeChain);
        buildContextAction?.Invoke(innerLogContext);
        return innerLogContext;
    }
    public IScopedLogContext CreateScope(IDictionary<string, object> state)
    {
        var innerLogContext = new ScopedLogContext(state, _scopeChain);
        return innerLogContext;
    }

    public IReadOnlyDictionary<string, object> GetValues() => _scopeChain.GetCombinedValue();

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _rootLogContext.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_rootLogContext).GetEnumerator();
    }

    public void Add(KeyValuePair<string, object> item)
    {
        _rootLogContext.Add(item);
    }

    public void Clear()
    {
        _rootLogContext.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
        return _rootLogContext.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        _rootLogContext.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
        return _rootLogContext.Remove(item);
    }

    public int Count => _rootLogContext.Count;

    public bool IsReadOnly => _rootLogContext.IsReadOnly;

    public void Add(string key, object value)
    {
        _rootLogContext.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _rootLogContext.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _rootLogContext.Remove(key);
    }

    public bool TryGetValue(string key, out object value)
    {
        return _rootLogContext.TryGetValue(key, out value);
    }

    public object this[string key]
    {
        get => _rootLogContext[key];
        set => _rootLogContext[key] = value;
    }

    public ICollection<string> Keys => _rootLogContext.Keys;

    public ICollection<object> Values => _rootLogContext.Values;

    public void Add(IDictionary<string, object> dictionary)
    {
        _rootLogContext.Add(dictionary);
    }

    public void Add(params (string, object)[] valueTuples)
    {
        _rootLogContext.Add(valueTuples);
    }

    public void Add(params Tuple<string, object>[] tuples)
    {
        _rootLogContext.Add(tuples);
    }

    public void Add(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
    {
        _rootLogContext.Add(keyValuePairs);
    }
    
    public void RemoveKeys(IEnumerable<string> keys)
    {
        _rootLogContext.RemoveKeys(keys);
    }
}