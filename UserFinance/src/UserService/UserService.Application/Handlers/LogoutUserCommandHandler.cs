using MediatR;
using Microsoft.Extensions.Logging;
using UserFinance.Common.Security;
using UserService.Abstractions.Persistence;
using UserService.Abstractions.Services;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class LogoutUserCommandHandler(ICurrentUserAccessor currentUserAccessor,
    IUserAuthService userAuthService, IUnitOfWork unitOfWork, ILogger<LogoutUserCommandHandler> logger)
    : IRequestHandler<LogoutUserCommand>
{
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await userAuthService.LogoutAsync(currentUserAccessor.JwtId, currentUserAccessor.ExpiresAtUtc,
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User logged out successfully.");
    }
}
