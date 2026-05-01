using MediatR;
using UserService.Api.Requests;
using UserService.Api.Responses;
using UserService.Application.Commands;

namespace UserService.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginUserRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new LoginUserCommand(request.Name, request.Password), cancellationToken);
            return Results.Ok(new AuthResponse(result.AccessToken));
        });

        group.MapPost("/logout", async (ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new LogoutUserCommand(), cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization();

        return group;
    }
}
