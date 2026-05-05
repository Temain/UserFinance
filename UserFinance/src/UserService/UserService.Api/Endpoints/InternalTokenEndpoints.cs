using UserService.Abstractions.Repositories;

namespace UserService.Api.Endpoints;

public static class InternalTokenEndpoints
{
    public static IEndpointRouteBuilder MapInternalTokenEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/internal/tokens/{jti}/revoked",
            async (string jti, IRevokedTokenRepository revokedTokenRepository, CancellationToken cancellationToken) =>
            {
                var isRevoked = await revokedTokenRepository.ExistsAsync(jti, cancellationToken);
                return Results.Ok(new
                {
                    isRevoked
                });
            }).WithTags("Internal").ExcludeFromDescription();

        return endpoints;
    }
}
