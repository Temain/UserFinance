using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace FinanceService.Api.Exceptions;

public sealed class FinanceServiceExceptionHandler : IExceptionHandler
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
            InvalidOperationException invalidOperationException => Results.BadRequest(new
            {
                error = invalidOperationException.Message
            }),
            _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
        };

        await result.ExecuteAsync(httpContext);
        return true;
    }
}
