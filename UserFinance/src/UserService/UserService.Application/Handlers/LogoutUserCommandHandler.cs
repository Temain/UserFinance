using MediatR;
using UserService.Application.Commands;

namespace UserService.Application.Handlers;

public sealed class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
{
    public Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
