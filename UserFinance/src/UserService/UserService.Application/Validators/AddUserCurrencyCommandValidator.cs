using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class AddUserCurrencyCommandValidator : AbstractValidator<AddUserCurrencyCommand>
{
    public AddUserCurrencyCommandValidator()
    {
        RuleFor(command => command.UserId).GreaterThan(0).WithMessage("User id must be greater than zero.");
        RuleFor(command => command.CurrencyId).GreaterThan(0).WithMessage("Currency id must be greater than zero.");
    }
}
