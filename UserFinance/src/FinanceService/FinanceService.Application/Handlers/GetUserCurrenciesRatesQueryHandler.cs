using FinanceService.Abstractions.Services;
using FinanceService.Application.Models;
using FinanceService.Application.Queries;
using MediatR;

namespace FinanceService.Application.Handlers;

public sealed class GetUserCurrenciesRatesQueryHandler(IFinanceService financeService)
    : IRequestHandler<GetUserCurrenciesRatesQuery, IReadOnlyCollection<CurrencyRateDto>>
{
    public async Task<IReadOnlyCollection<CurrencyRateDto>> Handle(GetUserCurrenciesRatesQuery request,
        CancellationToken cancellationToken)
    {
        var currencies = await financeService.GetUserCurrenciesAsync(request.UserId, cancellationToken);
        return currencies
            .Select(currency => new CurrencyRateDto(currency.Id, currency.Name, currency.Rate))
            .ToArray();
    }
}
