using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Password).NotEmpty().MaximumLength(200);
    }
}
