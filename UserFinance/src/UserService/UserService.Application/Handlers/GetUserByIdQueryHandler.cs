using MediatR;
using UserService.Abstractions.Services;
using UserService.Application.Models;
using UserService.Application.Queries;

namespace UserService.Application.Handlers;

public sealed class GetUserByIdQueryHandler(IUserProfileService userProfileService)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userProfileService.GetByIdAsync(request.UserId, cancellationToken);
        return user is null ? null : new UserDto(user.Id, user.Name);
    }
}
