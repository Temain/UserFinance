namespace UserService.Api.Requests;

public sealed record AddUserFavoritesRequest(IReadOnlyCollection<int> FavoriteCurrencyIds);
