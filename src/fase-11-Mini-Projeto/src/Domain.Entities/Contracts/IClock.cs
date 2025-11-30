namespace Domain.Entities.Contracts;

public interface IClock
{
    DateTime Now { get; }

    Task Delay(TimeSpan delay, CancellationToken ct = default);
}
