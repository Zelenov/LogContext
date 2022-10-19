using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using SharpLogContext.Abstractions;

namespace SharpLogContext;

/// <summary>
/// Decorates dictionary data within the scope boundaries
/// Adds itself into scopes chain on creation
/// Removes itself from scopes chain on dispose
/// </summary>
internal class ScopedLogContext : IScopedLogContext
{
    private readonly IDictionary<string, object> _innerStorage;
    private readonly IDisposable _scopeObject;

    public ScopedLogContext(ILogContextScopeChain scopeChain)
    {
        var storage = new ConcurrentDictionary<string, object>();
        _innerStorage = storage;
        _scopeObject = scopeChain.Push(storage); 
    }
    public ScopedLogContext(IDictionary<string, object> state, ILogContextScopeChain scopeChain)
    {
        var storage = new ConcurrentDictionary<string, object>(state);
        _innerStorage = storage;
        _scopeObject = scopeChain.Push(storage);
    }

    public void Add(IDictionary<string, object> dictionary)
    {
        if (dictionary == null)
            throw new ArgumentException(nameof(dictionary));
        Add((IEnumerable<KeyValuePair<string, object>>)dictionary);
    }
    
    public void Add(params ValueTuple<string, object>[] valueTuples)
    {
        if (valueTuples == null)
            throw new ArgumentException(nameof(valueTuples));
        Add(valueTuples.Select(t => new KeyValuePair<string, object>(t.Item1, t.Item2)));
    }

    public void Add(params Tuple<string, object>[] tuples)
    {
        if (tuples == null)
            throw new ArgumentException(nameof(tuples));
        Add(tuples.Select(t => new KeyValuePair<string, object>(t.Item1, t.Item2)));
    }
    public void Add(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
    {
        if (keyValuePairs == null)
            throw new ArgumentException(nameof(keyValuePairs));
        foreach (var keyValuePair in keyValuePairs)
        {
            var key = keyValuePair.Key;
            var value = keyValuePair.Value;
            if (!string.IsNullOrWhiteSpace(key))
                _innerStorage[key] = value;
        }
    }
    
    ///<inheritdoc/>
    public void RemoveKeys(IEnumerable<string> keys)
    {
        if (keys == null)
            throw new ArgumentException(nameof(keys));
        foreach (var key in keys)
                _innerStorage.Remove(key);
    }
    
    public void Add(string key, object value)
    {
        _innerStorage.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _innerStorage.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _innerStorage.Remove(key);
    }

    public bool TryGetValue(string key, out object value) => _innerStorage.TryGetValue(key, out value);

    public object this[string key]
    {
        get => _innerStorage[key];
        set => _innerStorage[key] = value;
    }

    public ICollection<string> Keys => _innerStorage.Keys;

    public ICollection<object> Values => _innerStorage.Values;
    
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _innerStorage.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_innerStorage).GetEnumerator();
    }

    public void Add(KeyValuePair<string, object> item)
    {
        _innerStorage.Add(item);
    }

    void ICollection<KeyValuePair<string, object>>.Clear()
    {
        _innerStorage.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
        return _innerStorage.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        _innerStorage.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
        return _innerStorage.Remove(item);
    }

    public int Count => _innerStorage.Count;

    public bool IsReadOnly => _innerStorage.IsReadOnly;

    public void Dispose()
    {
        _scopeObject?.Dispose();
    }
}
