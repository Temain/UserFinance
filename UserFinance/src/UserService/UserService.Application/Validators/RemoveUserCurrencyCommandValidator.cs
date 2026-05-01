using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class RemoveUserCurrencyCommandValidator : AbstractValidator<RemoveUserCurrencyCommand>
{
    public RemoveUserCurrencyCommandValidator()
    {
        RuleFor(command => command.UserId).GreaterThan(0);
        RuleFor(command => command.CurrencyId).GreaterThan(0);
    }
}
