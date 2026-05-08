using Prometheus;
using PrometheusMetrics = Prometheus.Metrics;

namespace FinanceService.Infrastructure.Metrics;

public static class UserServiceMetrics
{
    public static readonly Counter Requests = PrometheusMetrics.CreateCounter(
        "finance_user_service_requests_total",
        "Total number of requests from finance service to user service.",
        new CounterConfiguration
        {
            LabelNames = ["endpoint", "result"]
        });

    public static readonly Histogram RequestDuration = PrometheusMetrics.CreateHistogram(
        "finance_user_service_request_duration_seconds",
        "Duration of requests from finance service to user service in seconds.",
        new HistogramConfiguration
        {
            LabelNames = ["endpoint"]
        });
}
