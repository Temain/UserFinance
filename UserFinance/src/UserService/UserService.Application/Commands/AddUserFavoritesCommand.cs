using MediatR;

namespace UserService.Application.Commands;

public sealed record AddUserFavoritesCommand(long UserId, IReadOnlyCollection<int> FavoriteCurrencyIds) : IRequest;
