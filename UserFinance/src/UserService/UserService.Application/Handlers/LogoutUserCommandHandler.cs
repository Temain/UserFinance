using MediatR;
using UserFinance.Common.Security;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class LogoutUserCommandHandler(ICurrentUserAccessor currentUserAccessor,
    IUserAuthService userAuthService) : IRequestHandler<LogoutUserCommand>
{
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await userAuthService.LogoutAsync(currentUserAccessor.JwtId, currentUserAccessor.ExpiresAtUtc,
            cancellationToken);
    }
}
