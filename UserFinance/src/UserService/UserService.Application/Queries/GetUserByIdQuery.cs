using MediatR;
using UserService.Application.Models;

namespace UserService.Application.Queries;

public sealed record GetUserByIdQuery(long UserId) : IRequest<UserDto?>;
