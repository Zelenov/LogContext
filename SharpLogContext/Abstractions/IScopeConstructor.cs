using System;
using System.Collections.Generic;

namespace SharpLogContext.Abstractions;

public interface IScopeConstructor
{
    /// <summary>
    /// Adds temporary values to the output of <see cref="IGlobalLogContext.GetValues"/> method.
    /// Previous values are restored after disposing of context.
    /// </summary>
    IScopedLogContext CreateScope();

    /// <summary>
    /// Adds temporary values to the output of <see cref="IGlobalLogContext.GetValues"/> method.
    /// Previous values are restored after disposing of context.
    /// </summary>
    IScopedLogContext CreateScope(string key, object value);


    /// <summary>
    /// Adds temporary values to the output of <see cref="IGlobalLogContext.GetValues"/> method.
    /// Previous values are restored after disposing of context.
    /// </summary>
    IScopedLogContext CreateScope(IEnumerable<KeyValuePair<string, object>> keyValuePairs);

    /// <summary>
    /// Adds temporary values to the output of <see cref="IGlobalLogContext.GetValues"/> method.
    /// Previous values are restored after disposing of context.
    /// </summary>
    IScopedLogContext CreateScope(KeyValuePair<string, object> keyValuePair);

    /// <summary>
    /// Adds temporary values to the output of <see cref="IGlobalLogContext.GetValues"/> method.
    /// Previous values are restored after disposing of context.
    /// </summary>
    IScopedLogContext CreateScope(params ValueTuple<string, object>[] valueTuples);

    /// <summary>
    /// Adds temporary values to the output of <see cref="IGlobalLogContext.GetValues"/> method.
    /// Previous values are restored after disposing of context.
    /// </summary>
    IScopedLogContext CreateScope(params Tuple<string, object>[] tuples);

    /// <summary>
    /// Adds temporary values to the output of <see cref="IGlobalLogContext.GetValues"/> method.
    /// Previous values are restored after disposing of context.
    /// </summary>
    IScopedLogContext CreateScope(Action<ILogContext> buildContextAction);
}
