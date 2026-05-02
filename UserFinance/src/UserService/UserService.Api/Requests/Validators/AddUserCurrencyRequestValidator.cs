using FluentValidation;
using UserService.Api.Requests;

namespace UserService.Api.Requests.Validators;

public sealed class AddUserCurrencyRequestValidator : AbstractValidator<AddUserCurrencyRequest>
{
    public AddUserCurrencyRequestValidator()
    {
        RuleFor(request => request.CurrencyId).GreaterThan(0).WithMessage("Currency id must be greater than zero.");
    }
}
