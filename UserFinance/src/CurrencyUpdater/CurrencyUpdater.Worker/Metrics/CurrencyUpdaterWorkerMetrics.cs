using Prometheus;
using PrometheusMetrics = Prometheus.Metrics;

namespace CurrencyUpdater.Worker.Metrics;

public static class CurrencyUpdaterWorkerMetrics
{
    public static readonly Counter UpdateFailures = PrometheusMetrics.CreateCounter(
        "currency_updater_update_failures_total",
        "Total number of failed currency updater runs.");
}
