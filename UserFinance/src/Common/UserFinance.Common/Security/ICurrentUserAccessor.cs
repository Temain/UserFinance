namespace UserFinance.Common.Security;

public interface ICurrentUserAccessor
{
    long? UserId { get; }

    string? JwtId { get; }

    DateTimeOffset? ExpiresAtUtc { get; }

    string? AccessToken { get; }
}
