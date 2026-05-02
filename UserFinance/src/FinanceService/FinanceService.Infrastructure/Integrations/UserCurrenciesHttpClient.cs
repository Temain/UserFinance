using System.Net.Http.Json;
using FinanceService.Abstractions.Integrations;
using FinanceService.Infrastructure.Options;
using Microsoft.Extensions.Options;
using UserFinance.Common.Security;

namespace FinanceService.Infrastructure.Integrations;

public sealed class UserCurrenciesHttpClient(HttpClient httpClient, IOptions<UserServiceOptions> userServiceOptions,
    ICurrentUserAccessor currentUserAccessor) : IUserCurrenciesClient
{
    public async Task<IReadOnlyCollection<int>> GetUserCurrencyIdsAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        var requestPath = userServiceOptions.Value.UserCurrenciesPath.Replace("{userId}", userId.ToString());
        using var request = new HttpRequestMessage(HttpMethod.Get, requestPath);

        if (!string.IsNullOrWhiteSpace(currentUserAccessor.AccessToken))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                currentUserAccessor.AccessToken);
        }

        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var userCurrencies = await response.Content.ReadFromJsonAsync<UserCurrencyClientResponse[]>(
            cancellationToken: cancellationToken);

        return userCurrencies?.Select(userCurrency => userCurrency.CurrencyId).ToArray() ?? Array.Empty<int>();
    }

    private sealed record UserCurrencyClientResponse(long UserId, int CurrencyId);
}
