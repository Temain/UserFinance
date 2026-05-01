using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class RemoveUserCurrencyCommandHandler(IUserProfileService userProfileService)
    : IRequestHandler<RemoveUserCurrencyCommand>
{
    public async Task Handle(RemoveUserCurrencyCommand request, CancellationToken cancellationToken)
    {
        await userProfileService.RemoveCurrencyAsync(request.UserId, request.CurrencyId, cancellationToken);
    }
}
