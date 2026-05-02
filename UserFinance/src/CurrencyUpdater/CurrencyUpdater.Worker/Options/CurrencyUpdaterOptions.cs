namespace CurrencyUpdater.Worker.Options;

public sealed class CurrencyUpdaterOptions
{
    public const int DefaultUpdateIntervalMinutes = 60;

    public int UpdateIntervalMinutes { get; set; } = DefaultUpdateIntervalMinutes;
}
