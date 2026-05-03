using CurrencyUpdater.Application.Models;
using CurrencyUpdater.Application.Services;
using CurrencyUpdater.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CurrencyUpdater.Tests;

public sealed class CurrencyServiceTests
{
    [Fact]
    public async Task UpdateAsync_SplitsNewAndExistingCurrenciesAndSavesChanges()
    {
        var currencyRatesProvider = new FakeCurrencyRatesProvider("<xml />");
        var parsedRates = new[]
        {
            new CurrencyRateDto(840, "US Dollar", 80.5m),
            new CurrencyRateDto(978, "Euro", 92.1m),
            new CurrencyRateDto(156, "Yuan", 11.2m)
        };
        var currencyRatesParser = new FakeCurrencyRatesParser(parsedRates);
        var currencyWriteRepository = new FakeCurrencyWriteRepository(new Dictionary<int, CurrencyRateDto>
        {
            [840] = new(840, "US Dollar", 79.1m)
        });
        var service = new CurrencyService(currencyRatesProvider, currencyRatesParser, currencyWriteRepository,
            NullLogger<CurrencyService>.Instance);

        await service.UpdateAsync();

        Assert.Equal(["<xml />"], currencyRatesParser.ParsedXml);
        Assert.Equal([840, 978, 156], currencyWriteRepository.LastRequestedIds);
        Assert.Equal([978, 156], currencyWriteRepository.AddedCurrencies.Select(x => x.CurrencyId).ToArray());
        Assert.Equal([840], currencyWriteRepository.UpdatedCurrencies.Select(x => x.CurrencyId).ToArray());
        Assert.True(currencyWriteRepository.SaveChangesAsyncCalled);
    }
}
