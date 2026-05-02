namespace UserService.Api.Requests;

public sealed record AddUserCurrenciesRequest(IReadOnlyCollection<int> CurrencyIds);
