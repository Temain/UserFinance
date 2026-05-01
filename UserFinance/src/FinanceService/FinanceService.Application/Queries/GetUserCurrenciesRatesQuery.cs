using FinanceService.Application.Models;
using MediatR;

namespace FinanceService.Application.Queries;

public sealed record GetUserCurrenciesRatesQuery(long UserId) : IRequest<IReadOnlyCollection<CurrencyRateDto>>;
