namespace UserFinance.Common.Security;

public interface ICurrentUserAccessor
{
    long? UserId { get; }

    string? AccessToken { get; }
}
