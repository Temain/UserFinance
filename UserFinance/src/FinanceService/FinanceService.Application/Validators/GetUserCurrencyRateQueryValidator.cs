using FinanceService.Application.Queries;
using FluentValidation;

namespace FinanceService.Application.Validators;

public sealed class GetUserCurrencyRateQueryValidator : AbstractValidator<GetUserCurrencyRateQuery>
{
    public GetUserCurrencyRateQueryValidator()
    {
        RuleFor(query => query.UserId).GreaterThan(0).WithMessage("User id must be greater than zero.");
        RuleFor(query => query.CurrencyId).GreaterThan(0).WithMessage("Currency id must be greater than zero.");
    }
}
