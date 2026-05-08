using Prometheus;
using PrometheusMetrics = Prometheus.Metrics;

namespace CurrencyUpdater.Application.Metrics;

public static class CurrencyUpdaterMetrics
{
    public static readonly Counter UpdateRuns = PrometheusMetrics.CreateCounter(
        "currency_updater_update_runs_total",
        "Total number of currency updater runs.");

    public static readonly Counter UpdatedCurrencies = PrometheusMetrics.CreateCounter(
        "currency_updater_updated_currencies_total",
        "Total number of currencies processed by the updater.");

    public static readonly Gauge LastSuccessfulUpdate = PrometheusMetrics.CreateGauge(
        "currency_updater_last_successful_update_timestamp",
        "Unix timestamp of the last successful currency update.");

    public static readonly Histogram UpdateDuration = PrometheusMetrics.CreateHistogram(
        "currency_updater_update_duration_seconds",
        "Duration of the currency update operation in seconds.");
}
