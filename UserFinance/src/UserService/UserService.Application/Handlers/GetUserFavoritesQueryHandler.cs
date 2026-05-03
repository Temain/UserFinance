using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Models;
using UserService.Application.Queries;

namespace UserService.Application.Handlers;

public sealed class GetUserFavoritesQueryHandler(IUserProfileService userProfileService)
    : IRequestHandler<GetUserFavoritesQuery, IReadOnlyCollection<FavoriteCurrencyDto>>
{
    public async Task<IReadOnlyCollection<FavoriteCurrencyDto>> Handle(GetUserFavoritesQuery request,
        CancellationToken cancellationToken)
    {
        var favoriteCurrencies = await userProfileService.GetFavoriteCurrenciesAsync(request.UserId, cancellationToken);
        return favoriteCurrencies
            .Select(favoriteCurrency => new FavoriteCurrencyDto(favoriteCurrency.UserId, favoriteCurrency.CurrencyId))
            .ToArray();
    }
}
