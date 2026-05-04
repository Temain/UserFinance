namespace UserService.Domain.Exceptions;

public sealed class InvalidTokenException(string message = "Token metadata is invalid.")
    : InvalidOperationException(message)
{
}
