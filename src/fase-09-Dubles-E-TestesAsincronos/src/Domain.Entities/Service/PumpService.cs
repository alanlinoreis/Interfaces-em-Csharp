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

    // parâmetros configuráveis
    public int MaxRetries { get; init; } = 3; // número extra de tentativas (além da primeira)
    public TimeSpan InitialBackoff { get; init; } = TimeSpan.FromMilliseconds(50);
    public double BackoffFactor { get; init; } = 2.0; // exponencial

    public PumpService(IAsyncReader<T> reader, IAsyncWriter<T> writer, IClock clock)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    // RunAsync com retry/backoff e suporte a cancelamento
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
                    break; // sucesso
                }
                catch (OperationCanceledException) // preserve cancelamento
                {
                    throw;
                }
                catch (Exception)
                {
                    attempt++;

                    if (attempt > MaxRetries)
                    {
                        // excedeu tentativas → repropaga
                        throw;
                    }

                    // espera sem bloquear — usa relógio fake/real
                    await _clock.Delay(backoff, ct);

                    // exponencial
                    backoff = TimeSpan.FromMilliseconds(backoff.TotalMilliseconds * BackoffFactor);
                }
            }
        }
    }
}
