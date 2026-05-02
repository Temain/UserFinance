using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class AddUserCurrenciesCommandHandler(IUserProfileService userProfileService)
    : IRequestHandler<AddUserCurrenciesCommand>
{
    public async Task Handle(AddUserCurrenciesCommand request, CancellationToken cancellationToken)
    {
        await userProfileService.AddCurrenciesAsync(request.UserId, request.CurrencyIds, cancellationToken);
    }
}
