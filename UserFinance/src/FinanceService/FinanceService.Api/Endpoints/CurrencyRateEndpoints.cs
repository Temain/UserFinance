using FinanceService.Api.Extensions;
using FinanceService.Api.Responses;
using FinanceService.Application.Queries;
using MediatR;
using UserFinance.Common.Extensions;
using UserFinance.Common.Security;

namespace FinanceService.Api.Endpoints;

public static class CurrencyRateEndpoints
{
    public static IEndpointRouteBuilder MapCurrencyRateEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/users/{userId:long}/currency-rates").WithTags("CurrencyRates")
            .RequireAuthorization();

        group.MapGet(string.Empty,
            async (long userId, ICurrentUserAccessor currentUserAccessor, ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (!currentUserAccessor.HasAccessTo(userId))
                {
                    return Results.Forbid();
                }

                var currencyRates = await sender.Send(new GetUserCurrenciesRatesQuery(userId), cancellationToken);
                var response = currencyRates
                    .Select(currencyRate => new CurrencyRateResponse(currencyRate.CurrencyId,
                        currencyRate.CurrencyName, currencyRate.Rate));

                return Results.Ok(response);
            });

        group.MapGet("/{currencyId:int}",
            async (long userId, int currencyId, ICurrentUserAccessor currentUserAccessor, ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (!currentUserAccessor.HasAccessTo(userId))
                {
                    return Results.Forbid();
                }

                var currencyRate = await sender.Send(new GetUserCurrencyRateQuery(userId, currencyId),
                    cancellationToken);

                return currencyRate is null
                    ? Results.NotFound()
                    : Results.Ok(new CurrencyRateResponse(currencyRate.CurrencyId, currencyRate.CurrencyName,
                        currencyRate.Rate));
            });

        return endpoints;
    }
}
