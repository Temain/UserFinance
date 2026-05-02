using FluentValidation;
using UserService.Api.Requests;

namespace UserService.Api.Requests.Validators;

public sealed class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("User name is required.")
            .MaximumLength(200).WithMessage("User name must not be longer than 200 characters.");

        RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required.")
            .MaximumLength(200).WithMessage("Password must not be longer than 200 characters.");
    }
}
