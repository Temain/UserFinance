using FluentValidation;
using UserService.Application.Commands;

namespace UserService.Application.Validators;

public sealed class AddUserFavoritesCommandValidator : AbstractValidator<AddUserFavoritesCommand>
{
    public AddUserFavoritesCommandValidator()
    {
        RuleFor(command => command.UserId).GreaterThan(0).WithMessage("User id must be greater than zero.");

        RuleFor(command => command.FavoriteCurrencyIds)
            .NotEmpty()
            .WithMessage("At least one favorite currency id is required.");

        RuleFor(command => command.FavoriteCurrencyIds)
            .Must(currencyIds => currencyIds.Distinct().Count() == currencyIds.Count)
            .WithMessage("Favorite currency ids must be unique.");

        RuleForEach(command => command.FavoriteCurrencyIds)
            .GreaterThan(0)
            .WithMessage("Currency id must be greater than zero.");
    }
}
