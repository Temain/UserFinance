using FinanceService.Api.Endpoints;

namespace FinanceService.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapFinanceServiceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCurrencyRateEndpoints();
        return endpoints;
    }
}
