using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

using SharpLogContext.Abstractions;

namespace SharpLogContext;
/// <summary>
/// Stores scopes chain async-locally
/// returns combined data from the whole chain
/// based on Microsoft.Extensions.Logging.LoggerExternalScopeProvider
/// </summary>
internal class LoggerContextScopeChain : ILogContextScopeChain
{
    private readonly AsyncLocal<Scope> _currentScope = new ();

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, object> GetCombinedValue()
    {
        var last = _currentScope.Value;
        if (last == null)
            return new Dictionary<string, object>();

        if (last.Parent == null)
            return new ReadOnlyDictionary<string, object>(last.State);

        var res = new Dictionary<string, object>(last.State);
        var current = last.Parent;
        do
        {
            var values = current.State.ToArray();
            foreach (var value in values)
            {
                if (!res.ContainsKey(value.Key))
                    res.Add(value.Key, value.Value);
            }
            current = current.Parent;
        }
        while (current != null);

        return res;
    }
    
    /// <inheritdoc />
    public IDisposable Push(ConcurrentDictionary<string, object> state)
    {
        var parent = _currentScope.Value;
        var newScope = new Scope(this, state, parent);
        _currentScope.Value = newScope;

        return newScope;
    }
    
    private class Scope : IDisposable
    {
        private readonly LoggerContextScopeChain _chain;
        private bool _isDisposed;

        internal Scope(LoggerContextScopeChain chain, ConcurrentDictionary<string, object>  state, Scope parent)
        {
            _chain = chain;
            State = state;
            Parent = parent;
        }

        public Scope Parent { get; }
        public ConcurrentDictionary<string, object> State { get; }

        public override string ToString()
        {
            return State?.ToString();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _chain._currentScope.Value = Parent;
            _isDisposed = true;
        }
    }
}