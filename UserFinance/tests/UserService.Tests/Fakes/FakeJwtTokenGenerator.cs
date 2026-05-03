using UserService.Abstractions.Security;

namespace UserService.Tests.Fakes;

internal sealed class FakeJwtTokenGenerator : IJwtTokenGenerator
{
    public string LastUserName { get; private set; } = string.Empty;

    public string GenerateToken(long userId, string userName)
    {
        LastUserName = userName;
        return "jwt-token";
    }
}
