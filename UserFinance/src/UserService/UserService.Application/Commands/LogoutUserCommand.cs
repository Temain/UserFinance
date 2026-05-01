using MediatR;

namespace UserService.Application.Commands;

public sealed record LogoutUserCommand : IRequest;
