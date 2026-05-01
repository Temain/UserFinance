using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().WithMessage("User name is required.")
            .MaximumLength(200).WithMessage("User name must not be longer than 200 characters.");

        RuleFor(command => command.Password).NotEmpty().WithMessage("Password is required.")
            .MaximumLength(200).WithMessage("Password must not be longer than 200 characters.");
    }
}
