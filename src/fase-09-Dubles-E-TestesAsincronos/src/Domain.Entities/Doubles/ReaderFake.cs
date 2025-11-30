using Domain.Entities.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Entities.Doubles;

public class ReaderFake<T> : IAsyncReader<T>
{
    private readonly IEnumerable<T> _items;
    private readonly TimeSpan _gap;

    public ReaderFake(IEnumerable<T> items, TimeSpan? gap = null)
    {
        _items = items;
        _gap = gap ?? TimeSpan.Zero;
    }

    public async IAsyncEnumerable<T> ReadAllAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken ct = default)
    {
        foreach (var item in _items)
        {
            if (ct.IsCancellationRequested)
                yield break;

            if (_gap > TimeSpan.Zero)
                await Task.Delay(_gap, ct);

            yield return item;
        }
    }
}
