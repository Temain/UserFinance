using FluentValidation;
using FinanceService.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
            UserServiceIntegrationException userServiceIntegrationException => Results.Problem(
                statusCode: userServiceIntegrationException.StatusCode,
                title: "User service integration error",
                detail: userServiceIntegrationException.Message),
            InvalidOperationException invalidOperationException => Results.BadRequest(new
            {
                error = invalidOperationException.Message
            }),
            DbUpdateConcurrencyException => Results.Conflict(new
            {
                error = "The resource was modified by another operation."
            }),
            DbUpdateException => Results.Conflict(new
            {
                error = "The database update failed."
            }),
            _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
        };

        await result.ExecuteAsync(httpContext);
        return true;
    }
}
