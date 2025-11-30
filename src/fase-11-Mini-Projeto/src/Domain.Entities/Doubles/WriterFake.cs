using Domain.Entities.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Entities.Doubles;

public class WriterFake<T> : IAsyncWriter<T>
{
    public List<T> Written { get; } = new();

    public bool ShouldFail { get; set; } = false;

    /// <summary>
    /// FailAfter = -1 -> falha sempre
    /// FailAfter = 0  -> falha na primeira, depois funciona
    /// FailAfter = N  -> falha nas primeiras (N+1) tentativas
    /// </summary>
    public int FailAfter { get; set; } = -1;

    private int _attemptCount = 0;

    public Task WriteAsync(T item, CancellationToken ct = default)
    {
        // CANCELAMENTO SEMPRE VERIFICADO PRIMEIRO
        ct.ThrowIfCancellationRequested();

        _attemptCount++;

        // se não deve falhar
        if (!ShouldFail)
        {
            Written.Add(item);
            return Task.CompletedTask;
        }

        // falha sempre
        if (FailAfter < 0)
        {
            ct.ThrowIfCancellationRequested(); // re-check
            throw new Exception("WriterFake forced failure");
        }

        // falha nas primeiras tentativas
        if (_attemptCount <= FailAfter + 1)
        {
            ct.ThrowIfCancellationRequested(); // re-check ANTES da exceção
            throw new Exception("WriterFake forced failure");
        }

        // depois funciona
        Written.Add(item);
        return Task.CompletedTask;
    }
}
