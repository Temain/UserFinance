using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Models;
using UserService.Application.Queries;

namespace UserService.Application.Handlers;

public sealed class GetUserCurrenciesQueryHandler(IUserProfileService userProfileService)
    : IRequestHandler<GetUserCurrenciesQuery, IReadOnlyCollection<UserCurrencyDto>>
{
    public async Task<IReadOnlyCollection<UserCurrencyDto>> Handle(GetUserCurrenciesQuery request,
        CancellationToken cancellationToken)
    {
        var userCurrencies = await userProfileService.GetCurrenciesAsync(request.UserId, cancellationToken);
        return userCurrencies
            .Select(userCurrency => new UserCurrencyDto(userCurrency.UserId, userCurrency.CurrencyId))
            .ToArray();
    }
}
