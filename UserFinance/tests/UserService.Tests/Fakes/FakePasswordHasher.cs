using UserService.Abstractions.Security;

namespace UserService.Tests.Fakes;

internal sealed class FakePasswordHasher : IPasswordHasher
{
    public bool VerifyResult { get; init; } = true;

    public string Hash(string password)
    {
        return $"hashed-{password}";
    }

    public bool Verify(string password, string passwordHash)
    {
        return VerifyResult && passwordHash == $"hashed-{password}";
    }
}
