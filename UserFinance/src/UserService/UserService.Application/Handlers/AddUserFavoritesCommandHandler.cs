using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class AddUserFavoritesCommandHandler(IUserProfileService userProfileService)
    : IRequestHandler<AddUserFavoritesCommand>
{
    public async Task Handle(AddUserFavoritesCommand request, CancellationToken cancellationToken)
    {
        await userProfileService.AddFavoriteCurrenciesAsync(request.UserId, request.FavoriteCurrencyIds, cancellationToken);
    }
}
