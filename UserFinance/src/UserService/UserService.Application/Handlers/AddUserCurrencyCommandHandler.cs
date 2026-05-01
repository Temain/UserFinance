using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class AddUserCurrencyCommandHandler(IUserProfileService userProfileService)
    : IRequestHandler<AddUserCurrencyCommand>
{
    public async Task Handle(AddUserCurrencyCommand request, CancellationToken cancellationToken)
    {
        await userProfileService.AddCurrencyAsync(request.UserId, request.CurrencyId, cancellationToken);
    }
}
