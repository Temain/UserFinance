using FinanceService.Application.Models;
using MediatR;

namespace FinanceService.Application.Queries;

public sealed record GetUserCurrencyRateQuery(long UserId, int CurrencyId) : IRequest<CurrencyRateDto?>;
