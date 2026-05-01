using MediatR;
using UserService.Application.Models;

namespace UserService.Application.Commands;

public sealed record LoginUserCommand(string Name, string Password) : IRequest<AuthResponseDto>;
