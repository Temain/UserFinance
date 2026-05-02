using CurrencyUpdater.Application.Commands;
using CurrencyUpdater.Application.Services;
using MediatR;

namespace CurrencyUpdater.Application.Handlers;

public sealed class UpdateCurrenciesCommandHandler(ICurrencyService currencyService)
    : IRequestHandler<UpdateCurrenciesCommand>
{
    public Task Handle(UpdateCurrenciesCommand request, CancellationToken cancellationToken)
    {
        return currencyService.UpdateAsync(cancellationToken);
    }
}
