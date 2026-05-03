using Yarp.ReverseProxy.Configuration;

namespace ApiGateway.Extensions;

public static class ReverseProxyExtensions
{
    public static IServiceCollection AddApiGatewayReverseProxy(this IServiceCollection services,
        string userServiceUrl, string financeServiceUrl)
    {
        var routes = new[]
        {
            new RouteConfig
            {
                RouteId = "user-swagger",
                ClusterId = "user-service",
                Match = new RouteMatch
                {
                    Path = "/swagger/user/{**catch-all}"
                },
                Transforms = new[]
                {
                    new Dictionary<string, string>
                    {
                        ["PathRemovePrefix"] = "/swagger/user"
                    }
                },
                Order = -2
            },
            new RouteConfig
            {
                RouteId = "finance-swagger",
                ClusterId = "finance-service",
                Match = new RouteMatch
                {
                    Path = "/swagger/finance/{**catch-all}"
                },
                Transforms = new[]
                {
                    new Dictionary<string, string>
                    {
                        ["PathRemovePrefix"] = "/swagger/finance"
                    }
                },
                Order = -1
            },
            new RouteConfig
            {
                RouteId = "finance-favorite-currency-rates",
                ClusterId = "finance-service",
                Match = new RouteMatch
                {
                    Path = "/api/users/{userId}/favorites/rates"
                },
                AuthorizationPolicy = "default",
                Order = 0
            },
            new RouteConfig
            {
                RouteId = "finance-favorite-currency-rate",
                ClusterId = "finance-service",
                Match = new RouteMatch
                {
                    Path = "/api/users/{userId}/favorites/rates/{currencyId}"
                },
                AuthorizationPolicy = "default",
                Order = 0
            },
            new RouteConfig
            {
                RouteId = "auth",
                ClusterId = "user-service",
                Match = new RouteMatch
                {
                    Path = "/api/auth/{**catch-all}"
                },
                Order = 1
            },
            new RouteConfig
            {
                RouteId = "register-user",
                ClusterId = "user-service",
                Match = new RouteMatch
                {
                    Path = "/api/users",
                    Methods = new[] { "POST" }
                },
                Order = 2
            },
            new RouteConfig
            {
                RouteId = "users",
                ClusterId = "user-service",
                Match = new RouteMatch
                {
                    Path = "/api/users/{**catch-all}"
                },
                AuthorizationPolicy = "default",
                Order = 3
            }
        };

        var clusters = new[]
        {
            new ClusterConfig
            {
                ClusterId = "user-service",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["user-service"] = new()
                    {
                        Address = EnsureTrailingSlash(userServiceUrl)
                    }
                }
            },
            new ClusterConfig
            {
                ClusterId = "finance-service",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["finance-service"] = new()
                    {
                        Address = EnsureTrailingSlash(financeServiceUrl)
                    }
                }
            }
        };

        services.AddReverseProxy().LoadFromMemory(routes, clusters);

        return services;
    }

    private static string EnsureTrailingSlash(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new InvalidOperationException("Proxy destination address is not configured.");
        }

        return address.EndsWith('/') ? address : $"{address}/";
    }
}
