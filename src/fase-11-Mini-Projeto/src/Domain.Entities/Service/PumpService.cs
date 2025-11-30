using Domain.Entities.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Entities.Services;

public class PumpService<T>
{
    private readonly IAsyncReader<T> _reader;
    private readonly IAsyncWriter<T> _writer;
    private readonly IClock _clock;

    public int MaxRetries { get; init; } = 3;
    public TimeSpan InitialBackoff { get; init; } = TimeSpan.FromMilliseconds(50);
    public double BackoffFactor { get; init; } = 2.0;

    public PumpService(IAsyncReader<T> reader, IAsyncWriter<T> writer, IClock clock)
    {
        _reader = reader;
        _writer = writer;
        _clock = clock;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        await foreach (var item in _reader.ReadAllAsync(ct).WithCancellation(ct))
        {
            int attempt = 0;
            var backoff = InitialBackoff;

            while (true)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    await _writer.WriteAsync(item, ct);
                    break;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception)
                {
                    attempt++;

                    if (attempt > MaxRetries)
                        throw;

                    await _clock.Delay(backoff, ct);

                    // REFATORAÇÃO DA FASE 10
                    backoff = ProximoBackoff(backoff);
                }
            }
        }
    }

    // Novo método exigido pela Fase 10 — Antídoto
    private TimeSpan ProximoBackoff(TimeSpan atual)
    {
        return TimeSpan.FromMilliseconds(atual.TotalMilliseconds * BackoffFactor);
    }
}
