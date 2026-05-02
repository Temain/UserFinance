using FluentValidation;
using MediatR;
using UserService.Api.Extensions;
using UserService.Api.Requests;
using UserService.Api.Responses;
using UserService.Application.Abstractions.Services;
using UserService.Application.Commands;
using UserService.Application.Queries;

namespace UserService.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users").WithTags("Users");

        group.MapPost(string.Empty,
            async (RegisterUserRequest request, IValidator<RegisterUserRequest> validator, ISender sender,
                CancellationToken cancellationToken) =>
            {
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                var result = await sender.Send(new RegisterUserCommand(request.Name, request.Password),
                    cancellationToken);

                return Results.Ok(new AuthResponse(result.AccessToken));
            });

        group.MapGet("/{userId}",
            async (long userId, ICurrentUserAccessor currentUserAccessor, ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (!currentUserAccessor.HasAccessTo(userId))
                {
                    return Results.Forbid();
                }

                var user = await sender.Send(new GetUserByIdQuery(userId), cancellationToken);
                return user is null ? Results.NotFound() : Results.Ok(new UserResponse(user.Id, user.Name));
            }).RequireAuthorization();

        return group;
    }
}
