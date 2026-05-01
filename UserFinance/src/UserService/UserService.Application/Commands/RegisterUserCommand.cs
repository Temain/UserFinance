using MediatR;
using UserService.Application.Models;

namespace UserService.Application.Commands;

public sealed record RegisterUserCommand(string Name, string Password) : IRequest<AuthResponseDto>;
