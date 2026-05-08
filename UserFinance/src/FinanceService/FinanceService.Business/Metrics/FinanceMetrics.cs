using Prometheus;
using PrometheusMetrics = Prometheus.Metrics;

namespace FinanceService.Business.Metrics;

public static class FinanceMetrics
{
    public static readonly Counter FavoriteRatesRequests = PrometheusMetrics.CreateCounter(
        "finance_favorite_rates_requests_total",
        "Total number of requests for favorite currency rates.");

    public static readonly Counter SingleFavoriteRateRequests = PrometheusMetrics.CreateCounter(
        "finance_single_favorite_rate_requests_total",
        "Total number of requests for a single favorite currency rate.");

    public static readonly Counter SingleFavoriteRateMisses = PrometheusMetrics.CreateCounter(
        "finance_single_favorite_rate_misses_total",
        "Total number of single rate requests for currencies that are not in favorites.");
}
