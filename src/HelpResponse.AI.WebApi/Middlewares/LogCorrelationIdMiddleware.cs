using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpResponse.AI.WebApi.Middlewares;

public class LogCorrelationIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<LogCorrelationIdMiddleware> logger)
    {
        var scopeData = new Dictionary<string, object>
        {
            ["CorrelationId"] = Guid.NewGuid().ToString()
        };

        using (logger.BeginScope(scopeData))
        {
            await next(context);
        }
    }
}
