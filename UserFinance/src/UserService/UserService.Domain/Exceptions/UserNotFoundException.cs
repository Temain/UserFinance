namespace UserService.Domain.Exceptions;

public sealed class UserNotFoundException : InvalidOperationException
{
    public UserNotFoundException(long userId) : base($"User with id '{userId}' was not found.")
    {
    }
}
