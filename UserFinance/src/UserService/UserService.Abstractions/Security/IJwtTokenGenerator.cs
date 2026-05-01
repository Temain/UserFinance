namespace UserService.Abstractions.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(long userId, string userName);
}
