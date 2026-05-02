namespace FinanceService.Infrastructure.Options;

public sealed class UserServiceOptions
{
    public const string DefaultUserCurrenciesPath = "/api/users/{userId}/currencies";

    public string BaseUrl { get; set; } = string.Empty;

    public string UserCurrenciesPath { get; set; } = DefaultUserCurrenciesPath;
}
