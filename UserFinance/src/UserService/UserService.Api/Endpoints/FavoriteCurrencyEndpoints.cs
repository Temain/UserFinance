using FluentValidation;
using MediatR;
using UserFinance.Common.Extensions;
using UserService.Api.Requests;
using UserService.Api.Responses;
using UserService.Application.Commands;
using UserService.Application.Queries;

namespace UserService.Api.Endpoints;

public static class FavoriteCurrencyEndpoints
{
    public static RouteGroupBuilder MapFavoriteCurrencyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users/{userId:long}/favorites").WithTags("FavoriteCurrencies")
            .RequireAuthorization();

        group.MapGet(string.Empty,
            async (long userId, ISender sender, CancellationToken cancellationToken) =>
            {
                var favoriteCurrencies = await sender.Send(new GetUserFavoritesQuery(userId), cancellationToken);
                var response = favoriteCurrencies
                    .Select(favoriteCurrency => new FavoriteCurrencyResponse(favoriteCurrency.UserId,
                        favoriteCurrency.CurrencyId));

                return Results.Ok(response);
            }).RequireCurrentUserAccess();

        group.MapPost(string.Empty,
            async (long userId, AddUserFavoritesRequest request, IValidator<AddUserFavoritesRequest> validator,
                ISender sender, CancellationToken cancellationToken) =>
            {
                await validator.ValidateAndThrowAsync(request, cancellationToken);
                await sender.Send(new AddUserFavoritesCommand(userId, request.FavoriteCurrencyIds), cancellationToken);
                return Results.NoContent();
            }).RequireCurrentUserAccess();

        group.MapDelete("/{currencyId:int}",
            async (long userId, int currencyId, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(new RemoveUserFavoriteCommand(userId, currencyId), cancellationToken);
                return Results.NoContent();
            }).RequireCurrentUserAccess();

        return group;
    }
}
