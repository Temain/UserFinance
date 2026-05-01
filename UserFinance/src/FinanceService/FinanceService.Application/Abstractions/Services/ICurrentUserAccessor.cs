namespace FinanceService.Application.Abstractions.Services;

public interface ICurrentUserAccessor
{
    long? UserId { get; }
}
