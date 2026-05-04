namespace UserService.Domain.Entities;

public sealed class RevokedToken
{
    private RevokedToken()
    {
    }

    public RevokedToken(string jti, DateTime expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(jti))
        {
            throw new ArgumentException("Token jti is required.", nameof(jti));
        }

        Jti = jti;
        ExpiresAtUtc = expiresAtUtc;
    }

    public string Jti { get; private set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; private set; }
}
