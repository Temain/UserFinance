using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class RemoveUserFavoriteCommandHandler(IUserProfileService userProfileService)
    : IRequestHandler<RemoveUserFavoriteCommand>
{
    public async Task Handle(RemoveUserFavoriteCommand request, CancellationToken cancellationToken)
    {
        await userProfileService.RemoveFavoriteCurrencyAsync(request.UserId, request.CurrencyId, cancellationToken);
    }
}
