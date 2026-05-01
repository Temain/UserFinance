using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Commands;
using UserService.Application.Models;

namespace UserService.Application.Handlers;

public sealed class LoginUserCommandHandler(IUserAuthService userAuthService)
    : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var result = await userAuthService.LoginAsync(request.Name, request.Password, cancellationToken);
        return new AuthResponseDto(result.AccessToken);
    }
}
