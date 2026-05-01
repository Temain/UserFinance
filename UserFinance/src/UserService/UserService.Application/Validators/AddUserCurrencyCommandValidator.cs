using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class AddUserCurrencyCommandValidator : AbstractValidator<AddUserCurrencyCommand>
{
    public AddUserCurrencyCommandValidator()
    {
        RuleFor(command => command.UserId).GreaterThan(0);
        RuleFor(command => command.CurrencyId).GreaterThan(0);
    }
}
