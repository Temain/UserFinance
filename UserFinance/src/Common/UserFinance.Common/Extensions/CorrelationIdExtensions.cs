using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserFinance.Common.Observability;

namespace UserFinance.Common.Extensions;

public static class CorrelationIdExtensions
{
    public static IServiceCollection AddCorrelationId(this IServiceCollection services)
    {
        services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();
        return services;
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
