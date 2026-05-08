using System.Net.Http.Json;
using FinanceService.Abstractions.Integrations;
using FinanceService.Domain.Exceptions;
using FinanceService.Infrastructure.Metrics;
using FinanceService.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;
using UserFinance.Common.Observability;

namespace FinanceService.Infrastructure.Integrations;

public sealed class RevokedTokenHttpClient(HttpClient httpClient, IOptions<UserServiceOptions> userServiceOptions,
    ICorrelationIdAccessor correlationIdAccessor, ILogger<RevokedTokenHttpClient> logger) : IRevokedTokenClient
{
    public async Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken = default)
    {
        using var requestTimer = UserServiceMetrics.RequestDuration.WithLabels("revoked_token").NewTimer();
        var requestPath = userServiceOptions.Value.RevokedTokenPath.Replace("{jti}", Uri.EscapeDataString(jti));
        using var request = new HttpRequestMessage(HttpMethod.Get, requestPath);

        logger.LogInformation("Requesting revoked token status from user service for jti {JwtId}.", jti);

        if (!string.IsNullOrWhiteSpace(correlationIdAccessor.CorrelationId))
        {
            request.Headers.TryAddWithoutValidation(CorrelationIdConstants.HeaderName,
                correlationIdAccessor.CorrelationId);
        }

        HttpResponseMessage response;

        try
        {
            response = await httpClient.SendAsync(request, cancellationToken);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            UserServiceMetrics.Requests.WithLabels("revoked_token", "timeout").Inc();
            logger.LogWarning("User service revoked token request timed out for jti {JwtId}.", jti);
            throw new UserServiceIntegrationException("User service request timed out.",
                StatusCodes.Status503ServiceUnavailable);
        }
        catch (HttpRequestException exception)
        {
            UserServiceMetrics.Requests.WithLabels("revoked_token", "unavailable").Inc();
            logger.LogWarning(exception, "User service is unavailable while checking revoked token for jti {JwtId}.",
                jti);
            throw new UserServiceIntegrationException("User service is unavailable.",
                StatusCodes.Status503ServiceUnavailable, exception);
        }

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                UserServiceMetrics.Requests.WithLabels("revoked_token", "bad_status").Inc();
                logger.LogWarning("User service returned status code {StatusCode} for revoked token jti {JwtId}.",
                    (int)response.StatusCode, jti);
                throw new UserServiceIntegrationException(
                    $"User service returned unexpected status code {(int)response.StatusCode}.",
                    StatusCodes.Status502BadGateway);
            }

            var revokedTokenResponse = await response.Content.ReadFromJsonAsync<RevokedTokenResponse>(
                cancellationToken: cancellationToken);

            UserServiceMetrics.Requests.WithLabels("revoked_token", "success").Inc();
            logger.LogInformation("Received revoked token status from user service for jti {JwtId}.", jti);

            return revokedTokenResponse?.IsRevoked ?? false;
        }
    }

    private sealed record RevokedTokenResponse(bool IsRevoked);
}
