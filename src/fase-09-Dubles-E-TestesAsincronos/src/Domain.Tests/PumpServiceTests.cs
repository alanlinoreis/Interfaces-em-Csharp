using Xunit;
using Domain.Entities.Services;
using Domain.Entities.Doubles;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Domain.Tests;

public class PumpServiceTests
{
    [Fact]
    public async Task RunAsync_writes_all_items_when_writer_succeeds()
    {
        var reader = new ReaderFake<int>(new[] { 1, 2, 3 });
        var writer = new WriterFake<int>();
        var clock = new ClockFake();

        var pump = new PumpService<int>(reader, writer, clock);

        await pump.RunAsync();

        Assert.Equal(new[] { 1, 2, 3 }, writer.Written.ToArray());
    }

    [Fact]
    public async Task RunAsync_retries_and_succeeds_when_writer_recovers()
    {
        var reader = new ReaderFake<int>(new[] { 1, 2 });
        var writer = new WriterFake<int> { ShouldFail = true, FailAfter = 0 }; 
        // WriterFake will fail on first write, then succeed (FailAfter = 0 makes first fail)
        var clock = new ClockFake();

        var pump = new PumpService<int>(reader, writer, clock)
        {
            MaxRetries = 2,
            InitialBackoff = TimeSpan.FromMilliseconds(10),
            BackoffFactor = 2.0
        };

        await pump.RunAsync();

        // both items should be written: first item succeeds after retry, second item written normally
        Assert.Equal(new[] { 1, 2 }, writer.Written.ToArray());
    }

    [Fact]
    public async Task RunAsync_advances_clock_with_backoff_on_retries()
    {
        var reader = new ReaderFake<int>(new[] { 42 });
        var writer = new WriterFake<int> { ShouldFail = true, FailAfter = 0 };
        var clock = new ClockFake();

        var pump = new PumpService<int>(reader, writer, clock)
        {
            MaxRetries = 2,
            InitialBackoff = TimeSpan.FromMilliseconds(100),
            BackoffFactor = 2.0
        };

        var before = clock.Now;

        await pump.RunAsync();

        var elapsed = clock.Now - before;

        // initial 100ms backoff for first retry (then 200ms for next if needed); since FailAfter=0, only one retry occurs -> >=100ms
        Assert.True(elapsed >= TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    public async Task RunAsync_throws_after_exhausting_retries()
    {
        var reader = new ReaderFake<int>(new[] { 1 });
        var writer = new WriterFake<int> { ShouldFail = true, FailAfter = -1 }; // always fail
        var clock = new ClockFake();

        var pump = new PumpService<int>(reader, writer, clock)
        {
            MaxRetries = 1,
            InitialBackoff = TimeSpan.FromMilliseconds(1),
            BackoffFactor = 2.0
        };

        await Assert.ThrowsAsync<Exception>(() => pump.RunAsync(CancellationToken.None));
    }

    [Fact]
    public async Task RunAsync_cancels_during_backoff()
    {
        var reader = new ReaderFake<int>(new[] { 1 });
        var writer = new WriterFake<int> { ShouldFail = true, FailAfter = -1 }; // always fail
        var clock = new ClockFake();

        var pump = new PumpService<int>(reader, writer, clock)
        {
            MaxRetries = 5,
            InitialBackoff = TimeSpan.FromMilliseconds(500),
            BackoffFactor = 2.0
        };

        using var cts = new CancellationTokenSource();
        // cancel quickly during the first backoff
        var runTask = pump.RunAsync(cts.Token);

        // Cancel after small virtual delay — since ClockFake.Delay advances time only when called,
        // we cancel right away to simulate external cancellation before writer recovers.
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => runTask);
    }
}
