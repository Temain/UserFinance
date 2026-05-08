using Prometheus;
using PrometheusMetrics = Prometheus.Metrics;

namespace UserService.Business.Metrics;

public static class UserMetrics
{
    public static readonly Counter RegisterAttempts = PrometheusMetrics.CreateCounter(
        "user_register_attempts_total",
        "Total number of user registration attempts.");

    public static readonly Counter RegisterFailures = PrometheusMetrics.CreateCounter(
        "user_register_failures_total",
        "Total number of failed user registrations.");

    public static readonly Counter RegisteredUsers = PrometheusMetrics.CreateCounter(
        "user_registered_total",
        "Total number of successfully registered users.");

    public static readonly Counter LoginAttempts = PrometheusMetrics.CreateCounter(
        "user_login_attempts_total",
        "Total number of user login attempts.");

    public static readonly Counter LoginFailures = PrometheusMetrics.CreateCounter(
        "user_login_failures_total",
        "Total number of failed user logins.");

    public static readonly Counter SuccessfulLogins = PrometheusMetrics.CreateCounter(
        "user_login_success_total",
        "Total number of successful user logins.");

    public static readonly Counter Logouts = PrometheusMetrics.CreateCounter(
        "user_logout_total",
        "Total number of successful user logouts.");

    public static readonly Counter FavoriteCurrenciesAdded = PrometheusMetrics.CreateCounter(
        "user_favorite_currencies_added_total",
        "Total number of favorite currencies added by users.");

    public static readonly Counter FavoriteCurrenciesRemoved = PrometheusMetrics.CreateCounter(
        "user_favorite_currencies_removed_total",
        "Total number of favorite currencies removed by users.");

    public static readonly Histogram LoginDuration = PrometheusMetrics.CreateHistogram(
        "user_login_duration_seconds",
        "Duration of user login operation in seconds.");

    public static readonly Histogram AddFavoriteCurrenciesDuration = PrometheusMetrics.CreateHistogram(
        "user_add_favorite_currencies_duration_seconds",
        "Duration of adding favorite currencies in seconds.");
}
