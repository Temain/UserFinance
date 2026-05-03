using FluentValidation;
using UserService.Api.Requests;

namespace UserService.Api.Requests.Validators;

public sealed class AddUserFavoritesRequestValidator : AbstractValidator<AddUserFavoritesRequest>
{
    public AddUserFavoritesRequestValidator()
    {
        RuleFor(request => request.FavoriteCurrencyIds)
            .NotEmpty()
            .WithMessage("At least one favorite currency id is required.");

        RuleFor(request => request.FavoriteCurrencyIds)
            .Must(currencyIds => currencyIds.Distinct().Count() == currencyIds.Count)
            .WithMessage("Favorite currency ids must be unique.");

        RuleForEach(request => request.FavoriteCurrencyIds)
            .GreaterThan(0)
            .WithMessage("Currency id must be greater than zero.");
    }
}
