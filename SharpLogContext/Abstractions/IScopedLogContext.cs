using System;

namespace SharpLogContext.Abstractions;

/// <summary>
/// Log context stored in scopes chain. <inheritdoc cref="ILogContextScopeChain"/>
/// </summary>
public interface IScopedLogContext : ILogContext, IDisposable
{
}