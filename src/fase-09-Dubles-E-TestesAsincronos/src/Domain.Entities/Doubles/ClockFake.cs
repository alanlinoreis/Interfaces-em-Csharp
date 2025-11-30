using Domain.Entities.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Entities.Doubles;

public class ClockFake : IClock
{
    public DateTime Now { get; private set; } = DateTime.UtcNow;

    public Task Delay(TimeSpan delay, CancellationToken ct = default)
    {
        if (ct.IsCancellationRequested)
            return Task.FromCanceled(ct);

        Now = Now.Add(delay);
        return Task.CompletedTask;
    }
}
