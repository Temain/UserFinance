using MediatR;

namespace UserService.Application.Commands;

public sealed record RemoveUserFavoriteCommand(long UserId, int CurrencyId) : IRequest;
