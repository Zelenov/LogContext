# SharpLogContext
Stores log data throughout web request.

#Getting job done

Add `app.UseLogContext()` in the beginning of request pipeline in `Startup.cs`
```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseLogContext();
    ...
}
```

Add log items anywhere in your code

```csharp
public class FooController : ControllerBase
{
    [HttpGet]
    public void Get()
    {
        LogContext.Current.AttachValue("foo", true);
    }
}
```

Access your log items anywhere you want. Unlike logging scopes, sharing values only with the underlying calls, `LogContext` items can be accessed from the outside, even in the middleware.
```csharp
public class FooMiddleware
{
    private readonly RequestDelegate _next;
    public FooMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
        var foo = LogContext.Current.GetValue("foo");
    }
}
```

#Log it!
Extract `LogContext` items when you need to log them
```csharp
using Microsoft.Extensions.Logging;
...

private readonly ILogger<ImageProcessor> _logger;

...

var logContextValues = LogContext.Current.GetValues();
_logger.Log(logLevel: LogLevel.Info, eventId: 1, state: logContextValues, exception: null,
                formatter: (_, __) => "Log message");
```

#Scopes
Determine scope for particular log items, if you don't want them to be visible for the outer space.

```csharp
  LogContext.Current.AttachValue("1", 1);
  using (LogContext.Current.CreateScope("2", 2))
  {
      Console.WriteLine(LogContext.Current.GetValues());
      //Returns {"1", 1}, {"2", 2}
  }
  Console.WriteLine(LogContext.Current.GetValues());
  //Returns {"1", 1}
```
