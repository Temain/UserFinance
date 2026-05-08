using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Abstractions.Persistence;
using UserService.Abstractions.Security;
using UserService.Abstractions.Services;
using UserService.Application.Commands;
using UserService.Application.Models;

namespace UserService.Application.Handlers;

public sealed class RegisterUserCommandHandler(IUserAuthService userAuthService, IUnitOfWork unitOfWork,
    IJwtTokenGenerator jwtTokenGenerator, ILogger<RegisterUserCommandHandler> logger)
    : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userAuthService.RegisterAsync(request.Name, request.Password, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Name);
        logger.LogInformation("User {UserName} registered successfully.", request.Name);
        return new AuthResponseDto(accessToken);
    }
}
