using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Password).NotEmpty().MaximumLength(200);
    }
}
