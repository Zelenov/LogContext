using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SharpLogContext.Abstractions;

/// <summary>
/// Represents a storage of common scope data.
/// </summary>
public interface ILogContextScopeChain
{
    /// <summary>
    /// Merges all chain of dictionaries into one
    /// </summary>
    /// <returns>Combined dictionary, holding values of all dictionaries in scope chain</returns>
    IReadOnlyDictionary<string, object> GetCombinedValue();

    /// <summary>
    /// Adds scope object to the list
    /// </summary>
    /// <param name="state">The scope object</param>
    /// <returns>The <see cref="IDisposable"/> token that removes scope on dispose.</returns>
    IDisposable Push(ConcurrentDictionary<string, object> state);
}