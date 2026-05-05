namespace FinanceService.Infrastructure.Options;

public sealed class UserServiceOptions
{
    public const string DefaultUserFavoritesPath = "/api/users/{userId}/favorites";
    public const string DefaultRevokedTokenPath = "/internal/tokens/{jti}/revoked";

    public string BaseUrl { get; set; } = string.Empty;

    public string UserFavoritesPath { get; set; } = DefaultUserFavoritesPath;

    public string RevokedTokenPath { get; set; } = DefaultRevokedTokenPath;
}
