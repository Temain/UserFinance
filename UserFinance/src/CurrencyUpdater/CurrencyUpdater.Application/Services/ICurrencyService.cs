namespace CurrencyUpdater.Application.Services;

public interface ICurrencyService
{
    Task UpdateAsync(CancellationToken cancellationToken = default);
}
