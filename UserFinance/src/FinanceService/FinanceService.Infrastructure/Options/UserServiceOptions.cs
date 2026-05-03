namespace FinanceService.Infrastructure.Options;

public sealed class UserServiceOptions
{
    public const string DefaultUserFavoritesPath = "/api/users/{userId}/favorites";

    public string BaseUrl { get; set; } = string.Empty;

    public string UserFavoritesPath { get; set; } = DefaultUserFavoritesPath;
}
