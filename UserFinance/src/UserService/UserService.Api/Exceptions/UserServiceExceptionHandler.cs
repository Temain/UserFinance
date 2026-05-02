using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using UserService.Domain.Exceptions;

namespace UserService.Api.Exceptions;

public sealed class UserServiceExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var result = exception switch
        {
            ValidationException validationException => Results.ValidationProblem(
                validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray())),
            UserAlreadyExistsException userAlreadyExistsException => Results.Conflict(new
            {
                error = userAlreadyExistsException.Message
            }),
            InvalidCredentialsException invalidCredentialsException => Results.BadRequest(new
            {
                error = invalidCredentialsException.Message
            }),
            UserNotFoundException userNotFoundException => Results.NotFound(new
            {
                error = userNotFoundException.Message
            }),
            UserCurrencyAlreadyExistsException userCurrencyAlreadyExistsException => Results.Conflict(new
            {
                error = userCurrencyAlreadyExistsException.Message
            }),
            _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
        };

        await result.ExecuteAsync(httpContext);
        return true;
    }
}
