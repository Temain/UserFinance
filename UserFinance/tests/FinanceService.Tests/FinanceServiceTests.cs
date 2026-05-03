using FinanceService.Domain.Entities;
using FinanceService.Tests.Fakes;
using Xunit;
using FinanceDomainService = FinanceService.Business.Services.FinanceService;

namespace FinanceService.Tests;

public sealed class FinanceServiceTests
{
    [Fact]
    public async Task GetUserCurrenciesAsync_ReturnsCurrenciesForUserIds()
    {
        var currencyRepository = new FakeCurrencyRepository
        {
            CurrenciesByIds = [new Currency(840, "US Dollar", 80.5m), new Currency(978, "Euro", 92.1m)]
        };
        var userCurrenciesClient = new FakeUserCurrenciesClient([840, 978]);
        var service = new FinanceDomainService(currencyRepository, userCurrenciesClient);

        var result = await service.GetUserCurrenciesAsync(10);

        Assert.Equal(2, result.Count);
        Assert.Equal([840, 978], currencyRepository.LastRequestedIds);
    }

    [Fact]
    public async Task GetUserCurrencyAsync_WhenCurrencyDoesNotBelongToUser_ReturnsNull()
    {
        var currencyRepository = new FakeCurrencyRepository
        {
            CurrencyById = new Currency(840, "US Dollar", 80.5m)
        };
        var userCurrenciesClient = new FakeUserCurrenciesClient([978]);
        var service = new FinanceDomainService(currencyRepository, userCurrenciesClient);

        var result = await service.GetUserCurrencyAsync(10, 840);

        Assert.Null(result);
        Assert.False(currencyRepository.GetByIdAsyncCalled);
    }

    [Fact]
    public async Task GetUserCurrencyAsync_WhenCurrencyBelongsToUser_ReturnsCurrency()
    {
        var currencyRepository = new FakeCurrencyRepository
        {
            CurrencyById = new Currency(840, "US Dollar", 80.5m)
        };
        var userCurrenciesClient = new FakeUserCurrenciesClient([840, 978]);
        var service = new FinanceDomainService(currencyRepository, userCurrenciesClient);

        var result = await service.GetUserCurrencyAsync(10, 840);

        Assert.NotNull(result);
        Assert.Equal(840, result!.Id);
        Assert.True(currencyRepository.GetByIdAsyncCalled);
    }
}
