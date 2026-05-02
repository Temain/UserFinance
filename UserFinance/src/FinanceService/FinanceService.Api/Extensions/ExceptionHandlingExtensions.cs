using FinanceService.Api.Exceptions;

namespace FinanceService.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<FinanceServiceExceptionHandler>();

        return services;
    }
}
