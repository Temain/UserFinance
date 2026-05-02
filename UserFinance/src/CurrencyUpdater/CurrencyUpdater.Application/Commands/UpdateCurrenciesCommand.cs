using MediatR;

namespace CurrencyUpdater.Application.Commands;

public sealed record UpdateCurrenciesCommand : IRequest;
