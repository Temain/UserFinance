using MediatR;
using UserService.Application.Models;

namespace UserService.Application.Queries;

public sealed record GetUserCurrenciesQuery(long UserId) : IRequest<IReadOnlyCollection<UserCurrencyDto>>;
