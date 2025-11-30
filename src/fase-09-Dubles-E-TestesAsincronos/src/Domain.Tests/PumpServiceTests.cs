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
        var clock = new ClockFake();

        var pump = new PumpService<int>(reader, writer, clock)
        {
            MaxRetries = 2,
            InitialBackoff = TimeSpan.FromMilliseconds(10),
            BackoffFactor = 2.0
        };

        await pump.RunAsync();

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

        Assert.True(elapsed >= TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    public async Task RunAsync_throws_after_exhausting_retries()
    {
        var reader = new ReaderFake<int>(new[] { 1 });
        var writer = new WriterFake<int> { ShouldFail = true, FailAfter = -1 }; // falha sempre
        var clock = new ClockFake();

        var pump = new PumpService<int>(reader, writer, clock)
        {
            MaxRetries = 1,
            InitialBackoff = TimeSpan.FromMilliseconds(1),
            BackoffFactor = 2.0
        };

        await Assert.ThrowsAsync<Exception>(() => pump.RunAsync());
    }

}
