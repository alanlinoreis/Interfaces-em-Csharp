using Domain.Entities.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Entities.Doubles;

public class WriterFake<T> : IAsyncWriter<T>
{
    public List<T> Written { get; } = new();
    public bool ShouldFail { get; set; } = false;
    public int FailAfter { get; set; } = -1;

    private int _count = 0;

    public Task WriteAsync(T item, CancellationToken ct = default)
    {
        if (ShouldFail && (_count == FailAfter || FailAfter == -1))
        {
            _count++;
            throw new Exception("WriterFake forced failure");
        }

        Written.Add(item);
        _count++;
        return Task.CompletedTask;
    }
}
