using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Abstractions.Persistence;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class RemoveUserFavoriteCommandHandler(IUserProfileService userProfileService, IUnitOfWork unitOfWork,
    ILogger<RemoveUserFavoriteCommandHandler> logger) : IRequestHandler<RemoveUserFavoriteCommand>
{
    public async Task Handle(RemoveUserFavoriteCommand request, CancellationToken cancellationToken)
    {
        await userProfileService.RemoveFavoriteCurrencyAsync(request.UserId, request.CurrencyId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Removed favorite currency {CurrencyId} for user {UserId}.", request.CurrencyId,
            request.UserId);
    }
}
