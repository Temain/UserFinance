namespace CurrencyUpdater.Infrastructure.Options;

public sealed class CbrOptions
{
    public const string DefaultDailyRatesUrl = "https://www.cbr.ru/scripts/XML_daily.asp";

    public string DailyRatesUrl { get; set; } = DefaultDailyRatesUrl;
}
