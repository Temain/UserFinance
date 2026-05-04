namespace UserFinance.Common.Observability;

public interface ICorrelationIdAccessor
{
    string? CorrelationId { get; }
}
