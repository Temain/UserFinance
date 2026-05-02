using UserService.Api.Exceptions;

namespace UserService.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<UserServiceExceptionHandler>();

        return services;
    }
}
