using FluentValidation;
using UserService.Api.Requests;

namespace UserService.Api.Requests.Validators;

public sealed class AddUserCurrenciesRequestValidator : AbstractValidator<AddUserCurrenciesRequest>
{
    public AddUserCurrenciesRequestValidator()
    {
        RuleFor(request => request.CurrencyIds)
            .NotEmpty()
            .WithMessage("At least one currency id is required.");

        RuleFor(request => request.CurrencyIds)
            .Must(currencyIds => currencyIds.Distinct().Count() == currencyIds.Count)
            .WithMessage("Currency ids must be unique.");

        RuleForEach(request => request.CurrencyIds)
            .GreaterThan(0)
            .WithMessage("Currency id must be greater than zero.");
    }
}
