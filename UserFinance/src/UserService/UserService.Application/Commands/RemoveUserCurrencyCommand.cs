using MediatR;

namespace UserService.Application.Commands;

public sealed record RemoveUserCurrencyCommand(long UserId, int CurrencyId) : IRequest;
