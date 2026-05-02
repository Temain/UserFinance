using MediatR;

namespace UserService.Application.Commands;

public sealed record AddUserCurrenciesCommand(long UserId, IReadOnlyCollection<int> CurrencyIds) : IRequest;
