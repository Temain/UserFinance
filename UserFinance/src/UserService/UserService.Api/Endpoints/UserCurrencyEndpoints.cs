using FluentValidation;
using MediatR;
using UserFinance.Common.Extensions;
using UserFinance.Common.Security;
using UserService.Api.Extensions;
using UserService.Api.Requests;
using UserService.Api.Responses;
using UserService.Application.Commands;
using UserService.Application.Queries;

namespace UserService.Api.Endpoints;

public static class UserCurrencyEndpoints
{
    public static RouteGroupBuilder MapUserCurrencyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users/{userId:long}/currencies").WithTags("UserCurrencies")
            .RequireAuthorization();

        group.MapGet(string.Empty,
            async (long userId, ICurrentUserAccessor currentUserAccessor, ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (!currentUserAccessor.HasAccessTo(userId))
                {
                    return Results.Forbid();
                }

                var userCurrencies = await sender.Send(new GetUserCurrenciesQuery(userId), cancellationToken);
                var response = userCurrencies
                    .Select(userCurrency => new UserCurrencyResponse(userCurrency.UserId, userCurrency.CurrencyId));

                return Results.Ok(response);
            });

        group.MapPost(string.Empty,
            async (long userId, AddUserCurrencyRequest request, ICurrentUserAccessor currentUserAccessor,
                IValidator<AddUserCurrencyRequest> validator, ISender sender, CancellationToken cancellationToken) =>
            {
                if (!currentUserAccessor.HasAccessTo(userId))
                {
                    return Results.Forbid();
                }

                await validator.ValidateAndThrowAsync(request, cancellationToken);
                await sender.Send(new AddUserCurrencyCommand(userId, request.CurrencyId), cancellationToken);
                return Results.NoContent();
            });

        group.MapDelete("/{currencyId:int}",
            async (long userId, int currencyId, ICurrentUserAccessor currentUserAccessor, ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (!currentUserAccessor.HasAccessTo(userId))
                {
                    return Results.Forbid();
                }

                await sender.Send(new RemoveUserCurrencyCommand(userId, currencyId), cancellationToken);
                return Results.NoContent();
            });

        return group;
    }
}
