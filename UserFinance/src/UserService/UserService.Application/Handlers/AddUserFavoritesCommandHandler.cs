using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Abstractions.Persistence;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class AddUserFavoritesCommandHandler(IUserProfileService userProfileService, IUnitOfWork unitOfWork,
    ILogger<AddUserFavoritesCommandHandler> logger) : IRequestHandler<AddUserFavoritesCommand>
{
    public async Task Handle(AddUserFavoritesCommand request, CancellationToken cancellationToken)
    {
        await userProfileService.AddFavoriteCurrenciesAsync(request.UserId, request.FavoriteCurrencyIds,
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Added {FavoriteCurrencyCount} favorite currencies for user {UserId}.",
            request.FavoriteCurrencyIds.Count, request.UserId);
    }
}
