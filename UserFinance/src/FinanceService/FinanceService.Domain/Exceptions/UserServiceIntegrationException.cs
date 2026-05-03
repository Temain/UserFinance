namespace FinanceService.Domain.Exceptions;

public sealed class UserServiceIntegrationException(string message, int statusCode, Exception? innerException = null)
    : Exception(message, innerException)
{
    public int StatusCode { get; } = statusCode;
}
