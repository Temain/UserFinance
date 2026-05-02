using System.Globalization;
using System.Xml.Linq;
using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Application.Models;

namespace CurrencyUpdater.Infrastructure.Integrations;

public sealed class CbrCurrencyRatesXmlParser : ICurrencyRatesParser
{
    private static readonly CultureInfo RussianCulture = CultureInfo.GetCultureInfo("ru-RU");

    public IReadOnlyCollection<CurrencyRateDto> Parse(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new ArgumentException("Currency rates XML is required.", nameof(xml));
        }

        var document = XDocument.Parse(xml);

        return document.Root?
            .Elements("Valute")
            .Select(ParseCurrency)
            .OrderBy(currency => currency.CurrencyId)
            .ToArray()
            ?? Array.Empty<CurrencyRateDto>();
    }

    private static CurrencyRateDto ParseCurrency(XElement element)
    {
        var currencyId = int.Parse(GetRequiredValue(element, "NumCode"), CultureInfo.InvariantCulture);
        var currencyName = GetRequiredValue(element, "Name");
        var nominal = decimal.Parse(GetRequiredValue(element, "Nominal"), CultureInfo.InvariantCulture);
        var value = decimal.Parse(GetRequiredValue(element, "Value"), RussianCulture);
        var rate = decimal.Round(value / nominal, 6, MidpointRounding.AwayFromZero);

        return new CurrencyRateDto(currencyId, currencyName, rate);
    }

    private static string GetRequiredValue(XElement element, string elementName)
    {
        return element.Element(elementName)?.Value.Trim()
            ?? throw new InvalidOperationException($"Element '{elementName}' is missing in the CBR XML payload.");
    }
}
