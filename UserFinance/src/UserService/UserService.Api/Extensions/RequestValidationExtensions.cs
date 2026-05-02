using FluentValidation;
using UserService.Api.Requests.Validators;

namespace UserService.Api.Extensions;

public static class RequestValidationExtensions
{
    public static IServiceCollection AddRequestValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
        return services;
    }
}
