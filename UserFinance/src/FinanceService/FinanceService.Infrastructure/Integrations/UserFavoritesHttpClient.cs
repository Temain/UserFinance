using System.Net.Http.Headers;
using System.Net.Http.Json;
using FinanceService.Abstractions.Integrations;
using FinanceService.Domain.Exceptions;
using FinanceService.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UserFinance.Common.Security;

namespace FinanceService.Infrastructure.Integrations;

public sealed class UserFavoritesHttpClient(HttpClient httpClient, IOptions<UserServiceOptions> userServiceOptions,
    ICurrentUserAccessor currentUserAccessor) : IUserFavoritesClient
{
    public async Task<IReadOnlyCollection<int>> GetUserFavoriteCurrencyIdsAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        var requestPath = userServiceOptions.Value.UserFavoritesPath.Replace("{userId}", userId.ToString());
        using var request = new HttpRequestMessage(HttpMethod.Get, requestPath);

        if (!string.IsNullOrWhiteSpace(currentUserAccessor.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",
                currentUserAccessor.AccessToken);
        }

        HttpResponseMessage response;

        try
        {
            response = await httpClient.SendAsync(request, cancellationToken);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new UserServiceIntegrationException("User service request timed out.", StatusCodes.Status503ServiceUnavailable);
        }
        catch (HttpRequestException exception)
        {
            throw new UserServiceIntegrationException("User service is unavailable.", StatusCodes.Status503ServiceUnavailable, exception);
        }

        using (response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new UserServiceIntegrationException(
                    $"User service returned unexpected status code {(int)response.StatusCode}.",
                    StatusCodes.Status502BadGateway);
            }

            var favoriteCurrencies = await response.Content.ReadFromJsonAsync<FavoriteCurrencyClientResponse[]>(
                cancellationToken: cancellationToken);

            return favoriteCurrencies?.Select(favoriteCurrency => favoriteCurrency.CurrencyId).ToArray()
                ?? Array.Empty<int>();
        }
    }

    private sealed record FavoriteCurrencyClientResponse(long UserId, int CurrencyId);
}
