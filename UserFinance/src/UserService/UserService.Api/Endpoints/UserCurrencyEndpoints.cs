using FluentValidation;
using MediatR;
using UserFinance.Common.Extensions;
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
            async (long userId, ISender sender, CancellationToken cancellationToken) =>
            {
                var userCurrencies = await sender.Send(new GetUserCurrenciesQuery(userId), cancellationToken);
                var response = userCurrencies
                    .Select(userCurrency => new UserCurrencyResponse(userCurrency.UserId, userCurrency.CurrencyId));

                return Results.Ok(response);
            }).RequireCurrentUserAccess();

        group.MapPost(string.Empty,
            async (long userId, AddUserCurrenciesRequest request, IValidator<AddUserCurrenciesRequest> validator,
                ISender sender, CancellationToken cancellationToken) =>
            {
                await validator.ValidateAndThrowAsync(request, cancellationToken);
                await sender.Send(new AddUserCurrenciesCommand(userId, request.CurrencyIds), cancellationToken);
                return Results.NoContent();
            }).RequireCurrentUserAccess();

        group.MapDelete("/{currencyId:int}",
            async (long userId, int currencyId, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new RemoveUserCurrencyCommand(userId, currencyId), cancellationToken);
                return Results.NoContent();
            }).RequireCurrentUserAccess();

        return group;
    }
}
