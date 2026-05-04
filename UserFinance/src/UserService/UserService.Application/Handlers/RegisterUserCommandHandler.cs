using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Abstractions.Persistence;
using UserService.Abstractions.Services;
using UserService.Application.Commands;
using UserService.Application.Models;

namespace UserService.Application.Handlers;

public sealed class RegisterUserCommandHandler(IUserAuthService userAuthService, IUnitOfWork unitOfWork,
    ILogger<RegisterUserCommandHandler> logger) : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await userAuthService.RegisterAsync(request.Name, request.Password, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User {UserName} registered successfully.", request.Name);
        return new AuthResponseDto(result.AccessToken);
    }
}
