using FinanceService.Abstractions.Services;
using FinanceService.Application.Models;
using FinanceService.Application.Queries;
using MediatR;

namespace FinanceService.Application.Handlers;

public sealed class GetUserCurrencyRateQueryHandler(IFinanceService financeService)
    : IRequestHandler<GetUserCurrencyRateQuery, CurrencyRateDto?>
{
    public async Task<CurrencyRateDto?> Handle(GetUserCurrencyRateQuery request, CancellationToken cancellationToken)
    {
        var currency = await financeService.GetUserCurrencyAsync(request.UserId, request.CurrencyId, cancellationToken);
        return currency is null ? null : new CurrencyRateDto(currency.Id, currency.Name, currency.Rate);
    }
}
