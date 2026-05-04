using System.Net.Http.Headers;
using System.Net.Http.Json;
using FinanceService.Abstractions.Integrations;
using FinanceService.Domain.Exceptions;
using FinanceService.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserFinance.Common.Observability;
using UserFinance.Common.Security;

namespace FinanceService.Infrastructure.Integrations;

public sealed class UserFavoritesHttpClient(HttpClient httpClient, IOptions<UserServiceOptions> userServiceOptions,
    ICurrentUserAccessor currentUserAccessor, ICorrelationIdAccessor correlationIdAccessor,
    ILogger<UserFavoritesHttpClient> logger) : IUserFavoritesClient
{
    public async Task<IReadOnlyCollection<int>> GetUserFavoriteCurrencyIdsAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        var requestPath = userServiceOptions.Value.UserFavoritesPath.Replace("{userId}", userId.ToString());
        using var request = new HttpRequestMessage(HttpMethod.Get, requestPath);

        logger.LogInformation("Requesting favorite currencies from user service for user {UserId}.", userId);

        if (!string.IsNullOrWhiteSpace(currentUserAccessor.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",
                currentUserAccessor.AccessToken);
        }

        if (!string.IsNullOrWhiteSpace(correlationIdAccessor.CorrelationId))
        {
            request.Headers.TryAddWithoutValidation(CorrelationIdConstants.HeaderName, correlationIdAccessor.CorrelationId);
        }

        HttpResponseMessage response;

        try
        {
            response = await httpClient.SendAsync(request, cancellationToken);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("User service request timed out for user {UserId}.", userId);
            throw new UserServiceIntegrationException("User service request timed out.", StatusCodes.Status503ServiceUnavailable);
        }
        catch (HttpRequestException exception)
        {
            logger.LogWarning(exception, "User service is unavailable while fetching favorites for user {UserId}.", userId);
            throw new UserServiceIntegrationException("User service is unavailable.", StatusCodes.Status503ServiceUnavailable,
                exception);
        }

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("User service returned status code {StatusCode} for user {UserId}.",
                    (int)response.StatusCode, userId);
                throw new UserServiceIntegrationException(
                    $"User service returned unexpected status code {(int)response.StatusCode}.",
                    StatusCodes.Status502BadGateway);
            }

            var favoriteCurrencies = await response.Content.ReadFromJsonAsync<FavoriteCurrencyClientResponse[]>(
                cancellationToken: cancellationToken);

            logger.LogInformation("Received favorite currencies from user service for user {UserId}.", userId);

            return favoriteCurrencies?.Select(favoriteCurrency => favoriteCurrency.CurrencyId).ToArray()
                ?? Array.Empty<int>();
        }
    }

    private sealed record FavoriteCurrencyClientResponse(long UserId, int CurrencyId);
}
