using System.Collections.Generic;

namespace SharpLogContext.Abstractions;

/// <summary>
/// LogContext storing multiple scopes
/// </summary>
public interface IGlobalLogContext
{
    /// <summary>
    /// Returns all values from all current log scopes into one dictionary.
    /// </summary>
    IReadOnlyDictionary<string, object> GetValues();
}