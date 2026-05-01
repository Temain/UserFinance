using MediatR;

namespace UserService.Application.Commands;

public sealed record AddUserCurrencyCommand(long UserId, int CurrencyId) : IRequest;
