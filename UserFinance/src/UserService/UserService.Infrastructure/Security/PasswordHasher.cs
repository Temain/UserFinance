using Microsoft.AspNetCore.Identity;
using UserService.Abstractions.Security;

namespace UserService.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> passwordHasher = new();

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(password));
        }

        return passwordHasher.HashPassword(new object(), password);
    }

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        var verificationResult = passwordHasher.VerifyHashedPassword(new object(), passwordHash, password);
        return verificationResult is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
