using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Commands;
using UserService.Application.Models;

namespace UserService.Application.Handlers;

public sealed class RegisterUserCommandHandler(IUserAuthService userAuthService)
    : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await userAuthService.RegisterAsync(request.Name, request.Password, cancellationToken);
        return new AuthResponseDto(result.AccessToken);
    }
}
