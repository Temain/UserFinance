namespace CurrencyUpdater.Application.Models;

public sealed record CurrencyRateDto(int CurrencyId, string CurrencyName, decimal Rate);
