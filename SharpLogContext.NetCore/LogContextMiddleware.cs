using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SharpLogContext.NetCore
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            LogContext.Initialize();
            return _next.Invoke(context);
        }
    }
}
