using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class AddUserCurrenciesCommandValidator : AbstractValidator<AddUserCurrenciesCommand>
{
    public AddUserCurrenciesCommandValidator()
    {
        RuleFor(command => command.UserId).GreaterThan(0).WithMessage("User id must be greater than zero.");

        RuleFor(command => command.CurrencyIds)
            .NotEmpty()
            .WithMessage("At least one currency id is required.");

        RuleFor(command => command.CurrencyIds)
            .Must(currencyIds => currencyIds.Distinct().Count() == currencyIds.Count)
            .WithMessage("Currency ids must be unique.");

        RuleForEach(command => command.CurrencyIds)
            .GreaterThan(0)
            .WithMessage("Currency id must be greater than zero.");
    }
}
