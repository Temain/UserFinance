using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace UserService.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler(options => options.ExceptionHandlingPath = "/error");
        return services;
    }

    public static IEndpointRouteBuilder MapExceptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.Map("/error", (HttpContext httpContext) =>
        {
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            return exception switch
            {
                ValidationException validationException => Results.ValidationProblem(
                    validationException.Errors
                        .GroupBy(error => error.PropertyName)
                        .ToDictionary(group => group.Key,
                            group => group.Select(error => error.ErrorMessage).ToArray())),
                InvalidOperationException invalidOperationException => Results.BadRequest(new
                {
                    error = invalidOperationException.Message
                }),
                _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
            };
        }).ExcludeFromDescription();

        return endpoints;
    }
}
