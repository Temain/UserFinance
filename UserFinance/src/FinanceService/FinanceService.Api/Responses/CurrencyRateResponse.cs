namespace FinanceService.Api.Responses;

public sealed record CurrencyRateResponse(int CurrencyId, string CurrencyName, decimal Rate);
