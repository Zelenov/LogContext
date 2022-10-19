using System;

using Microsoft.Extensions.Logging;

namespace SharpLogContext;

/// <summary>
/// ILogger extension methods for common scenarios. LogContext.Current scope is applied to the log.
/// </summary>
public static class LoggerExtensions
{
    //------------------------------------------DEBUG------------------------------------------//

    /// <summary>
    /// Formats and writes a debug log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedDebug(0, exception, "Error while processing request from {Address}", address)</example>
    public static void LogDebugScoped(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Debug, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a debug log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedDebug(0, "Processing request from {Address}", address)</example>
    public static void LogDebugScoped(this ILogger logger, EventId eventId, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Debug, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a debug log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedDebug(exception, "Error while processing request from {Address}", address)</example>
    public static void LogDebugScoped(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Debug, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a debug log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedDebug("Processing request from {Address}", address)</example>
    public static void LogDebugScoped(this ILogger logger, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Debug, message, args);
    }

    //------------------------------------------TRACE------------------------------------------//

    /// <summary>
    /// Formats and writes a trace log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedTrace(0, exception, "Error while processing request from {Address}", address)</example>
    public static void LogTraceScoped(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Trace, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a trace log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedTrace(0, "Processing request from {Address}", address)</example>
    public static void LogTraceScoped(this ILogger logger, EventId eventId, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Trace, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a trace log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedTrace(exception, "Error while processing request from {Address}", address)</example>
    public static void LogTraceScoped(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Trace, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a trace log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedTrace("Processing request from {Address}", address)</example>
    public static void LogTraceScoped(this ILogger logger, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Trace, message, args);
    }

    //------------------------------------------INFORMATION------------------------------------------//

    /// <summary>
    /// Formats and writes an informational log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedInformation(0, exception, "Error while processing request from {Address}", address)</example>
    public static void LogInformationScoped(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Information, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an informational log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedInformation(0, "Processing request from {Address}", address)</example>
    public static void LogInformationScoped(this ILogger logger, EventId eventId, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Information, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes an informational log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedInformation(exception, "Error while processing request from {Address}", address)</example>
    public static void LogInformationScoped(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Information, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an informational log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedInformation("Processing request from {Address}", address)</example>
    public static void LogInformationScoped(this ILogger logger, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Information, message, args);
    }

    //------------------------------------------WARNING------------------------------------------//

    /// <summary>
    /// Formats and writes a warning log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedWarning(0, exception, "Error while processing request from {Address}", address)</example>
    public static void LogWarningScoped(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Warning, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a warning log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedWarning(0, "Processing request from {Address}", address)</example>
    public static void LogWarningScoped(this ILogger logger, EventId eventId, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Warning, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a warning log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedWarning(exception, "Error while processing request from {Address}", address)</example>
    public static void LogWarningScoped(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Warning, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a warning log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedWarning("Processing request from {Address}", address)</example>
    public static void LogWarningScoped(this ILogger logger, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Warning, message, args);
    }

    //------------------------------------------ERROR------------------------------------------//

    /// <summary>
    /// Formats and writes an error log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedError(0, exception, "Error while processing request from {Address}", address)</example>
    public static void LogErrorScoped(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Error, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an error log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedError(0, "Processing request from {Address}", address)</example>
    public static void LogErrorScoped(this ILogger logger, EventId eventId, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Error, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes an error log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedError(exception, "Error while processing request from {Address}", address)</example>
    public static void LogErrorScoped(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Error, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an error log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedError("Processing request from {Address}", address)</example>
    public static void LogErrorScoped(this ILogger logger, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Error, message, args);
    }

    //------------------------------------------CRITICAL------------------------------------------//

    /// <summary>
    /// Formats and writes a critical log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedCritical(0, exception, "Error while processing request from {Address}", address)</example>
    public static void LogCriticalScoped(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Critical, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a critical log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedCritical(0, "Processing request from {Address}", address)</example>
    public static void LogCriticalScoped(this ILogger logger, EventId eventId, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Critical, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a critical log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedCritical(exception, "Error while processing request from {Address}", address)</example>
    public static void LogCriticalScoped(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Critical, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a critical log message. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogScopedCritical("Processing request from {Address}", address)</example>
    public static void LogCriticalScoped(this ILogger logger, string message, params object[] args)
    {
        logger.LogScoped(LogLevel.Critical, message, args);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void LogScoped(this ILogger logger, LogLevel logLevel, string message, params object[] args)
    {
        logger.LogScoped(logLevel, 0, null, message, args);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void LogScoped(this ILogger logger, LogLevel logLevel, EventId eventId, string message, params object[] args)
    {
        logger.LogScoped(logLevel, eventId, null, message, args);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void LogScoped(this ILogger logger, LogLevel logLevel, Exception exception, string message, params object[] args)
    {
        logger.LogScoped(logLevel, 0, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a log message at the specified log level. LogContext.Current scope is applied to the log.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    public static void LogScoped(this ILogger logger, LogLevel logLevel, EventId eventId, Exception exception, string message, params object[] args)
    {
        if (logger == null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        using var scope = logger.BeginScope(LogContext.Current.GetValues());
        logger.Log(logLevel, eventId, exception, message, args);
    }
}
