using Microsoft.AspNetCore.Http;

namespace UserFinance.Common.Observability;

public sealed class CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor) : ICorrelationIdAccessor
{
    public string? CorrelationId
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return null;
            }

            if (httpContext.Items.TryGetValue(CorrelationIdConstants.ItemKey, out var correlationId))
            {
                return correlationId as string;
            }

            var headerValue = httpContext.Request.Headers[CorrelationIdConstants.HeaderName].ToString();
            return string.IsNullOrWhiteSpace(headerValue) ? null : headerValue;
        }
    }
}
