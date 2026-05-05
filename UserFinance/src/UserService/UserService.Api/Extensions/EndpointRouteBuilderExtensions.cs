using UserService.Api.Endpoints;

namespace UserService.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapUserServiceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAuthEndpoints();
        endpoints.MapUserEndpoints();
        endpoints.MapFavoriteCurrencyEndpoints();
        endpoints.MapInternalTokenEndpoints();

        return endpoints;
    }
}
