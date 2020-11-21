using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace LogContext
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add LogContext Middleware for storing log data between requests
        /// </summary>
        /// <param name="app">ApplicationBuilder</param>
        /// <returns>ApplicationBuilder</returns>
        public static IApplicationBuilder UseLogContext(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogContextMiddleware>();
            return app;
        }
    }
}
