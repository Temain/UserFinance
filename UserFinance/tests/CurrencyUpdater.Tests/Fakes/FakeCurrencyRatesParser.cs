using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Application.Models;

namespace CurrencyUpdater.Tests.Fakes;

internal sealed class FakeCurrencyRatesParser(IReadOnlyCollection<CurrencyRateDto> currencyRates) : ICurrencyRatesParser
{
    public List<string> ParsedXml { get; } = [];

    public IReadOnlyCollection<CurrencyRateDto> Parse(string xml)
    {
        ParsedXml.Add(xml);
        return currencyRates;
    }
}
