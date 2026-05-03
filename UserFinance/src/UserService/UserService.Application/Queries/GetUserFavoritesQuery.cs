using MediatR;
using UserService.Application.Models;

namespace UserService.Application.Queries;

public sealed record GetUserFavoritesQuery(long UserId) : IRequest<IReadOnlyCollection<FavoriteCurrencyDto>>;
