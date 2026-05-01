using UserService.Domain.Entities;

namespace UserService.Abstractions.Models;

public sealed record AuthenticationResult(User User, string AccessToken);
