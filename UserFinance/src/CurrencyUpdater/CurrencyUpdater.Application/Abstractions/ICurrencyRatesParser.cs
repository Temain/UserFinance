using CurrencyUpdater.Application.Models;

namespace CurrencyUpdater.Application.Abstractions;

public interface ICurrencyRatesParser
{
    IReadOnlyCollection<CurrencyRateDto> Parse(string xml);
}
