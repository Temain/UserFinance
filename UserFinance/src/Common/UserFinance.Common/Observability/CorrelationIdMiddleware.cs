using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace UserFinance.Common.Observability;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var correlationId = httpContext.Request.Headers[CorrelationIdConstants.HeaderName].ToString();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        httpContext.Items[CorrelationIdConstants.ItemKey] = correlationId;
        httpContext.Response.Headers[CorrelationIdConstants.HeaderName] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object?>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await next(httpContext);
        }
    }
}
