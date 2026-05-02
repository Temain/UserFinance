namespace UserService.Domain.Exceptions;

public sealed class InvalidCredentialsException : InvalidOperationException
{
    public InvalidCredentialsException() : base("Invalid user credentials.")
    {
    }
}
